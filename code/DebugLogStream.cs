using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class DebugLogTraceListener : TraceListener
{
    private StreamWriter logStream = new StreamWriter(new FileStream("network trace.txt", FileMode.Create));
    private bool logToConsole = false;
    
    [RuntimeInitializeOnLoadMethod]
    public static void EnableTraceLog()
    {
        if (StartupConfig.GetValue("EnableNetworkTraceLog") == "true")
        {
            var d = new DebugLogTraceListener
            {
                logToConsole = StartupConfig.GetValue("NetworkTraceLogToConsole") == "true"
            };
            Trace.Listeners.Add(d);
            Application.quitting += () => d.logStream.Dispose();
        }
    }
    
    public override void Write(string message)
    {
        if (logToConsole)
            Debug.Log(message);
        logStream.Write(message);
        logStream.Flush();
    }

    public override void WriteLine(string message)
    {
        if (logToConsole)
            Debug.Log(message);
        logStream.WriteLine(message);
        logStream.Flush();
    }
}

public class DebugLogStream : Stream
{
    public override void Flush()
    {
        
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return -1;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return offset;
    }

    public override void SetLength(long value)
    {
        
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        Debug.Log(Encoding.Default.GetString(buffer, offset, count));
    }

    public override bool CanRead => true;
    public override bool CanSeek => true;
    public override bool CanWrite => true;
    public override long Length => 9999999999;
    public override long Position { get; set; }
}
