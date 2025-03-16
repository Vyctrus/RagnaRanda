using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Windows.Forms;
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;

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
    string lastToInvert = "1";
    bool shouldInvert = false;
    string inversionType = "43";
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
                case 5:
                    // Inversion flow
                    if (shouldInvert)
                    {
                        UseInverted(false);
                        shouldInvert = false;
                        break;
                    }
                    ChangeInversionType();

                    //Standard flow
                    globalIndex = AnalyzeDrumSequenceSynch(currentSequence, globalIndex);
                    if(globalIndex >= currentSequence.Length)
                    {
                        currentSequence = GetRandomSequence();
                        globalIndex = 0;
                    }
                    break;
                case 6:
                    UseInverted(true);
                    shouldInvert = true;                 
                    break;
                case 7:
                    SendSingleTap();
                    break;
                case 8:
                    SendDoubleTap();
                   break;

            }
        }
        base.WndProc(ref m);
    }

    private void SendDoubleTap()
    {
        var tempC = random.Next(4) + 1;
        //2 double drums cannot be the same
        var tempD = (tempC + random.Next(3)+1) % 4;
        if (tempD == 0)
        {
            tempD = 4;
        }
        SendKeys.SendWait($"{tempC}");
        SendKeys.SendWait($"{tempD}");
        lastToInvert = $"{tempC}{tempD}";
    }

    private void SendSingleTap()
    {
        var tempC = random.Next(4) + 1;
        SendKeys.SendWait($"{tempC}");
        lastToInvert = $"{tempC}";
    }

    private void ChangeInversionType()
    {
        int firstNumber = random.Next(4) + 1;
        int secondNumber = random.Next(3) + 1;
        inversionType = $"{firstNumber}{secondNumber}";
        //Debug.WriteLine($"ChangeInversionType:{inversionType}");
    }

    private string InvertWith(string value, string howToInvert)
    {
        //Debug.WriteLine($"InvertWith: value:{value}, howToInvert:{howToInvert}");
        var firstDrum = 1;
        var secondDrum = 1;
        var x = int.Parse(value[0].ToString());
        var y = int.Parse(howToInvert[0].ToString());
        firstDrum = (x+y) % 4 + 1;
        if (value.Length == 2)
        {
            //2 double drums cannot be the same
            var b = int.Parse(howToInvert[1].ToString());
            //if (b > 3) { throw new Exception("me debil?"); }
            secondDrum = (firstDrum + b) % 4;
            if(secondDrum == 0)
            {
                secondDrum = 4;
            }
            //if(firstDrum == secondDrum)
            //{
            //    throw new Exception("me idiot?");
            //}
            return $"{firstDrum}{secondDrum}";
        }
        return $"{firstDrum}";
    }

    private void UseInverted(bool isInverted)
    {
        string input = lastToInvert;
        if (isInverted)
        {
            input = InvertWith(lastToInvert, inversionType);
        }

        foreach (var drum in input)
        {
            SendKeys.SendWait($"{drum}");
        }
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

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        // Unregister the hotkey when the form is closed
        UnregisterHotKey(this.Handle, 5);
        UnregisterHotKey(this.Handle, 6);
        UnregisterHotKey(this.Handle, 7);
        UnregisterHotKey(this.Handle, 8);
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
                lastToInvert = $"{c}";
                break;
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
                    lastToInvert = $"{c}{secondDrum}";
                    break;
            }
        }
        index++;
        return index;    
    }

    private void StartRhytmKeysTranslation()
    {   
        //AnalyzeDrumSequence(LoadSequence(0));
        RegisterHotKey(this.Handle, 5, MOD_NONE, (int)Keys.Z);
        RegisterHotKey(this.Handle, 6, MOD_NONE, (int)Keys.X);
        RegisterHotKey(this.Handle, 7, MOD_NONE, (int)Keys.N);
        RegisterHotKey(this.Handle, 8, MOD_NONE, (int)Keys.M);
        StartSongWithSpacebar();
    }

    private void button3_Click(object sender, EventArgs e)
    {
        StartRhytmKeysTranslation();
    }
}
