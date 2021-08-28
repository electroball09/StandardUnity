using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImGuiNET;
using System;
using System.Text;

public class DearImguiConsole : MonoBehaviour
{
    static manager inst { get; set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init2()
    {
        inst = new manager();
        Application.logMessageReceived += Application_logMessageReceived;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        GameObject obj = new GameObject("DearImguiConsole");
        obj.AddComponent<DearImguiConsole>();
        DontDestroyOnLoad(obj);
    }

    private static void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        inst?.addLog(condition, stackTrace, type);
    }

    class manager
    {
        public List<LogEntry> logs = new List<LogEntry>();

        public float scrollY = 0f;
        public float maxScrollY = 0f;
        public bool scrollToBottom = true;
        string statusText = "";
        DateTime lastStatus = DateTime.Now;

        public void addLog(string condition, string stackTrace, LogType type)
        {
            stackTrace = "   " + stackTrace.Replace("\n", "\n   ");
            logs.Add(new LogEntry(condition, stackTrace, type, false, DateTime.Now));

            if (scrollY >= maxScrollY)
                scrollToBottom = true;
        }

        public void PushStatus(string status)
        {
            statusText = status;
            lastStatus = DateTime.Now;
        }

        public void DrawStatusImgui()
        {
            float time = (float)(1 - ((DateTime.Now - lastStatus).TotalSeconds / 1));
            Vector4 col = new Vector4(1, 1, 1, time);
            ImGui.PushStyleColor(ImGuiCol.Text, col);
            ImGui.Text(statusText);
            ImGui.PopStyleColor();
        }
    }

    class LogEntry
    {
        string log;
        public string stackTrace;
        public LogType logType;
        public bool isOpen;
        public DateTime time;

        public string logStrNoTimestamp;
        public string logStr;
        
        public LogEntry(string _log, string _stackTrace, LogType _logType, bool _isOpen, DateTime _time)
        {
            log = _log;
            stackTrace = _stackTrace;
            logType = _logType;
            isOpen = _isOpen;
            time = _time;

            logStrNoTimestamp = $"({logType}) {log}";
            logStr = $"[{time.ToShortTimeString()}] {logStrNoTimestamp}";
        }
    }

    public bool showErrors = true;
    public bool showWarnings = true;
    public bool showInfo = true;
    public bool timestamps = true;
    public TimeSpan CmdFadeTime = TimeSpan.FromSeconds(1);
    public int selectedLog = 0;
    public string consoleInput = "";
    public string desiredConsoleInput = "";
    public string lastConsoleInput = "";
    public bool FocusText = false;
    public List<string> history = new List<string>();
    public int historyLogIndex = -1;

    void Start()
    {
        ImGuiUn.Layout += Layout;

        CursorDefault.ConsoleToggled += CursorDefault_ConsoleToggled;

        ComReg.AddCom(this, "history", (int val) =>
        {
            if (val < history.Count && val >= 0)
                Debug.Log($"history[{val}] - {history[val]}");
        });

        ComReg.AddCom("clear", () =>
        {
            inst.logs.Clear();
        });
    }

    void Update()
    {
        if (!CursorDefault.consoleIsOpen) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            NavHistoryUp();
        if (Input.GetKeyDown(KeyCode.DownArrow))
            NavHistoryDown();
    }

    private void CursorDefault_ConsoleToggled(bool toggledOn)
    {
        if (toggledOn)
            FocusText = true;
    }

    private void NavHistoryUp()
    {
        if (historyLogIndex == -1)
            historyLogIndex = history.Count - 1;
        else if (historyLogIndex != 0)
            historyLogIndex--;

        SetHistory();
    }

    private void NavHistoryDown()
    {
        if (historyLogIndex == -1) return;

        if (historyLogIndex == history.Count - 1)
            historyLogIndex = -1;
        else
            historyLogIndex++;

        SetHistory();
    }

    private void SetHistory()
    {
        if (historyLogIndex != -1)
            desiredConsoleInput = history[historyLogIndex];
        else
            desiredConsoleInput = "";
    }

    unsafe void Layout()
    {
        if (!CursorDefault.consoleIsOpen)
        {
            //ImGui.ShowDemoWindow();
            return;
        }

        ImGui.SetNextWindowSize(new Vector2(Screen.width, 200));
        ImGui.SetNextWindowPos(new Vector2(0, 0));
        if(ImGui.Begin("window", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoSavedSettings))
        {
            ImGui.Separator();

            if (ImGui.BeginMenuBar())
            {
                ImGui.Checkbox("Errors", ref showErrors);
                ImGui.Checkbox("Warnings", ref showWarnings);
                ImGui.Checkbox("Info", ref showInfo);

                ImGui.Spacing();

                ImGui.Checkbox("Timestamps", ref timestamps);

                ImGui.EndMenuBar();
            }

            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(0, 0));
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, Vector2.one);
            if (ImGui.BeginChildFrame(1, ImGui.GetWindowSize() - new Vector2(0, 60)))
            {
                uint idx = 0;
                foreach (var log in inst.logs)
                {
                    string str = timestamps ? log.logStr : log.logStrNoTimestamp;
                    ImGui.PushStyleVar(ImGuiStyleVar.ItemInnerSpacing, Vector2.zero);

                    TimeSpan diff = DateTime.Now - log.time;
                    float alpha = 1 - Mathf.Clamp01((float)diff.TotalSeconds - (float)CmdFadeTime.TotalSeconds);

                    Vector4 col = Vector4.one;
                    if (log.logType == LogType.Assert || log.logType == LogType.Exception)
                        col = new Vector4(.6f, 0, 0, 1);
                    if (log.logType == LogType.Error)
                        col = new Vector4(1, 0, 0, 1);
                    if (log.logType == LogType.Warning)
                        col = new Vector4(1, 1, .3f, 1);
                    if (log.logType == LogType.Log)
                        col = new Vector4(1, 1, 1, 1);
                    ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(1, 1, 1, 0.1f * alpha));
                    ImGui.PushStyleColor(ImGuiCol.Text, col);
                    Vector2 size = new Vector2(ImGui.GetWindowWidth(), ImGui.CalcTextSize(str).y + 3);
                    ImGui.PushStyleVar(ImGuiStyleVar.ButtonTextAlign, new Vector2(0f, 0.5f));
                    bool show = ((log.logType == LogType.Assert || log.logType == LogType.Exception) && showErrors)
                        || (log.logType == LogType.Error && showErrors)
                        || (log.logType == LogType.Warning && showWarnings)
                        || (log.logType == LogType.Log && showInfo);
                    if (show)
                    {
                        if (ImGui.Button(str, size))
                        {
                            log.isOpen = !log.isOpen;
                        }
                        if (ImGui.BeginPopupContextItem($"expand_log_{idx}"))
                        {
                            ImGui.Checkbox("Stack trace", ref log.isOpen);
                            if (ImGui.Selectable("Copy..."))
                            {
                                if (log.isOpen)
                                    ImGui.SetClipboardText($"{str}\n{log.stackTrace}");
                                else
                                    ImGui.SetClipboardText(str);
                                inst.PushStatus("Copied!");
                            }

                            ImGui.EndPopup();
                        }
                        if (log.isOpen)
                        {
                            ImGui.Text(log.stackTrace);
                        }
                    }
                    ImGui.PopStyleColor();
                    ImGui.PopStyleColor();
                    ImGui.PopStyleVar();
                    ImGui.PopStyleVar();

                    idx++;
                }

                inst.scrollToBottom = ImGui.GetScrollY() >= ImGui.GetScrollMaxY();

                if (inst.scrollToBottom)
                {
                    ImGui.SetScrollHereY(1.0f);
                }

                ImGui.EndChildFrame();
            }
            ImGui.PopStyleVar();
            ImGui.PopStyleVar();

            ImGui.Spacing();

            ImGuiInputTextCallback cb = (ImGuiInputTextCallbackData* data) =>
            {
                ImGuiInputTextCallbackDataPtr ptr = new ImGuiInputTextCallbackDataPtr(data);
                if (desiredConsoleInput != null)
                {
                    ptr.DeleteChars(0, ptr.BufTextLen);
                    ptr.InsertChars(0, desiredConsoleInput);
                    desiredConsoleInput = null;
                }
                return ptr.BufTextLen;
            };
            if (ImGui.InputTextWithHint("", ">", ref consoleInput, 150, ImGuiInputTextFlags.EnterReturnsTrue | ImGuiInputTextFlags.CallbackAlways, cb))
            {
                bool didRun = ComReg.RunCom(consoleInput);
                //if (history.Count == 0 || history[history.Count - 1] != consoleInput)
                if (didRun)
                    history.Add(consoleInput);
                consoleInput = "";
                historyLogIndex = -1;
                ImGui.SetKeyboardFocusHere();
            }

            if (FocusText)
            {
                ImGui.SetKeyboardFocusHere(-1);
                FocusText = false;
            }
            if (consoleInput.Contains("`")) //hack
                consoleInput = consoleInput.Replace("`", "");
            ImGui.SameLine();
            inst.DrawStatusImgui();

            ImGui.End();
        }
    }
}
