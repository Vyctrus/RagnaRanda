using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Windows.Forms;
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;

namespace WinFormsApp1;

public partial class Form1 : Form
{
    // P/Invoke declarations
    const int WM_HOTKEY = 0x0312;
    const int MOD_NONE = 0x0000;
    const int MOD_ALT = 0x0001;
    const int MOD_CONTROL = 0x0002;
    const int MOD_SHIFT = 0x0004;

    int globalIndex = 0;
    //string currentSequence = null;
    string currentSequence;
    Random random = new Random();

    // Register the hotkey
    [DllImport("user32.dll")]
    public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

    // Unregister the hotkey
    [DllImport("user32.dll")]
    public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    // Handle the Windows Message loop to detect hotkeys
    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_HOTKEY)
        {
            int hotkeyId = m.WParam.ToInt32();
            switch (hotkeyId)
            {
                // I dont think i need dupa translator anymore tbh
                //case 1:
                //    SendKeys.SendWait("{4 1}");
                //    // MessageBox.Show("'T' key pressed globally! Action triggered.");
                //    break;
                //case 2:
                //    SendKeys.SendWait("{3 1}");
                //    break;
                //case 3:
                //    SendKeys.SendWait("{2 1}");
                //    break;
                //case 4:
                //    SendKeys.SendWait("{1 1}");
                //    break;
                case 5:
                    globalIndex = AnalyzeDrumSequenceSynch(currentSequence, globalIndex);
                    if(globalIndex >= currentSequence.Length)
                    {
                        currentSequence = GetRandomSequence();
                        globalIndex = 0;
                        //throw new IndexOutOfRangeException();
                    }
                    break;
            }
        }

        base.WndProc(ref m);
    }

    public Form1()
    {
        InitializeComponent();
        // you can choose your own sequnce here
        //currentSequence = LoadSequence(2);
        currentSequence = GetRandomSequence();
    }

    private string GetRandomSequence()
    {
        int randomIndex = random.Next(DrumSequence.testSequences.Length);
        return LoadSequence(randomIndex);
    }

    private void OnKeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == 't' || e.KeyChar == 'T')
        {
            SendKeys.SendWait("{3 1}");
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        Thread.Sleep(2000);
        TestRunBot(null);
    }

    private static void TestRunBot(string[] args)
    {
        Random rand = new Random();
        // Display message to inform the user that the bot is running
        Console.WriteLine("Bot is running... It will start typing in the active window.");

        // Run the bot for 5 seconds
        DateTime endTime = DateTime.Now.AddSeconds(5);

        while (DateTime.Now < endTime)
        {
            // Randomly select a key (1, 2, 3, 4)
            int key = rand.Next(1, 5); // Random number between 1 and 4

            // Simulate the key press
            SendKeyPress(key);

            // Wait for a random interval (between 100ms and 1000ms)
            Thread.Sleep(rand.Next(100, 1001));
        }

        Console.WriteLine("Bot has finished typing.");
    }

    private static void SendKeyPress(int key)
    {
        switch (key)
        {
            case 1:
                //SendKeys.Send("1");
                SendKeys.SendWait("{1 5}");
                break;
            case 2:
                SendKeys.SendWait("{2 5}");
                break;
            case 3:
                SendKeys.SendWait("{3 5}");
                break;
            case 4:
                SendKeys.SendWait("{4 5}");
                break;
            default:
                break;
        }
        Console.WriteLine($"Key {key} pressed.");
    }



    private void button2_Click(object sender, EventArgs e)
    {
        //Thread.Sleep(1000);

        // Register the hotkey (T key in this case)
        //RegisterHotKey(this.Handle, 1, MOD_NONE, (int)Keys.D);
        //RegisterHotKey(this.Handle, 2, MOD_NONE, (int)Keys.U);
        //RegisterHotKey(this.Handle, 3, MOD_NONE, (int)Keys.P);
        //RegisterHotKey(this.Handle, 4, MOD_NONE, (int)Keys.A);

        RegisterHotKey(this.Handle, 5, MOD_NONE, (int)Keys.Z);

        StartSongWithSpacebar();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        // Unregister the hotkey when the form is closed
        UnregisterHotKey(this.Handle, 1);
        UnregisterHotKey(this.Handle, 2);
        UnregisterHotKey(this.Handle, 3);
        UnregisterHotKey(this.Handle, 4);
        UnregisterHotKey(this.Handle, 5);
        base.OnFormClosing(e);
    }

    private void StartSongWithSpacebar()
    {
        Thread.Sleep(3000);
        SendKeys.SendWait(" ");
    }

    private string LoadSequence(int index)
    {
        if (index >= DrumSequence.testSequences.Length || index < 0)
        {
            throw new NotImplementedException("There is no DrumSequence under that index");
        }
        foreach (var c in DrumSequence.testSequences[index])
        {
            // If the character is not one of the allowed characters, throw an exception
            if (c != '1' && c != '2' && c != '3' && c != '4' && c != ',')
            {
                throw new NotImplementedException("DrumSequence contains invalid characters.");
            }
        }
        return DrumSequence.testSequences[index];
    }

    private void AnalyzeDrumSequence(string drumSequenceData)
    {
        foreach (var c in drumSequenceData)
        {
            switch (c)
            {
                case '1':
                case '2':
                case '3':
                case '4':
                    SendKeys.SendWait($"{c}");
                    break;
                case ',':
                    SendKeys.SendWait(" ");
                    break;
            }
        }
    }

    private int AnalyzeDrumSequenceSynch(string drumSequenceData, int index)
    {
        var c = drumSequenceData[index];
        switch (c)
        {
            case '1':
            case '2':
            case '3':
            case '4':
                SendKeys.SendWait($"{c}");
                break;
            //case ',':
            //    SendKeys.SendWait(" ");
            //    break;
        }

        // Is double drum?
        if(index+1 < drumSequenceData.Length)
        {
            var secondDrum = drumSequenceData[index + 1];
            switch (secondDrum)
            {
                case ',':
                    //skip if no 2nd drum
                    index++;
                    break;
                case '1':
                case '2':
                case '3':
                case '4':
                    SendKeys.SendWait($"{secondDrum}");
                    index += 2;
                    break;
            }
        }
        index++;
        return index;    
    }

    private void TestRun()
    {
        StartSongWithSpacebar();
        //AnalyzeDrumSequence(LoadSequence(0));
        RegisterHotKey(this.Handle, 5, MOD_NONE, (int)Keys.Z);
    }

    private void button3_Click(object sender, EventArgs e)
    {
        TestRun();
    }
}
