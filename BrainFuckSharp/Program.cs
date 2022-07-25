using System.CommandLine;

using BrainFuckSharp;

Console.WriteLine("Brainfuck# compiler");

var compiler = new Compiler();

var memoryLimit = new Option<int>("-m", "Memory limit for program. Default is 4096")
{
    IsRequired = false,
    AllowMultipleArgumentsPerToken = false,
};

var @namespace = new Option<string>("-ns", "Namespace of program")
{
    IsRequired = false,
    AllowMultipleArgumentsPerToken = false,
};

var inputFile = new Option<string>("-i", "Input brainfuck file")
{
    IsRequired = true,
    AllowMultipleArgumentsPerToken = false,
};

var output = new Option<string>("-o", "Output assembly name")
{
    IsRequired = true,
    AllowMultipleArgumentsPerToken = false,
};

memoryLimit.SetDefaultValue(4096);
@namespace.SetDefaultValue("BrainFuckProgram");

var compilerCommand = new RootCommand();
compilerCommand.Add(memoryLimit);
compilerCommand.Add(@namespace);
compilerCommand.Add(inputFile);
compilerCommand.Add(output);

//compilerCommand.SetHandler(DoCompile);


compilerCommand.SetHandler((memoryLimitValue, @namespaceValue, inputFileValue, outputValue) =>
{
    var options = new BrainFuckSharp.CompilerOptions
    {
        MemoryLimit = memoryLimitValue,
        Namespace = @namespaceValue,
        BrainFuckSource = File.ReadAllText(inputFileValue),
        OutputFileName = outputValue
    };

    compiler.Compile(options);

}, memoryLimit, @namespace, inputFile, output);