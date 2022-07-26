
using BrainFuckSharp;
using BrainFuckSharp.Lib;

if (args.Length < 1)
{
    Console.WriteLine("usage: BrainFuckSharp program.bf");
    return;
}

JitBrainFuckInterpreter jitBrainFuckInterpreter = new(new SystemConsole());

jitBrainFuckInterpreter.Execute(args[0]);