# Tools
**Both files are required to work (Tools.dll, WinAPI.dll)**

Useful features:

- Power management 
> Example:
> ```cs
> PowerManagement.Set(PowerAction.Shutdown);
> ```
- Math
> Math class
> - Clamp
> > Clamp value to min and max
> > 
> > Example:
> > ```cs
> > int value = Math.Clamp(value: 120, max: 100, min: 0); // it will return 100
> > ```
> - Sin
> > Generate sinusoid wave
> > 
> > Example:
> > ```cs
> > short[] sin = Math.Sin(rate: 500, bufferSize: 2000); // That will generate array with sinusoid values 
> > ```
- Timeout
> class to set timeout for action 
> 
> Constructor arguments: TimeoutTime time
> - Start
> > Arguments: Action action 
> > 
> > start action after timeout without blocking thread 
>
> Usage:
> ```cs
> Timeout t = new Timeout(new TimeoutTime
> {
>     Seconds = 10
> });
> t.Start(() => {
>     Console.WriteLine("That will be shown after 20 seconds after calling \"Start\" method!");
> } 
> Console.WriteLine("That will execure withour timeout!");
> ```
- MessageBox
> Show message box to user 
> 
> Example:
> ```cs
> MessageBox.Show("Hello, world", "My box", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
> ```


