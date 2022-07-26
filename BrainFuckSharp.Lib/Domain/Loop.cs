using System.Collections;

namespace BrainFuckSharp.Lib.Domain
{
    internal sealed class Loop : IInstruction, IEquatable<Loop?>, IEnumerable<IInstruction>
    {
        public IList<IInstruction> Instructions { get; set; }

        public Loop()
        {
            Instructions = new List<IInstruction>();
        }

        public void Add(IInstruction instruction)
        {
            Instructions.Add(instruction);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Loop);
        }

        public bool Equals(Loop? other)
        {
            if (other == null
                || other.Instructions.Count != Instructions.Count)
            {
                return false;
            }

            for (int i = 0; i < Instructions.Count; i++)
            {
                if (!Instructions[i].Equals(other.Instructions[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            HashCode hash = new();
            foreach (IInstruction? item in Instructions)
            {
                hash.Add(item);
            }
            return hash.ToHashCode();
        }

        public IEnumerator<IInstruction> GetEnumerator()
        {
            return Instructions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Instructions.GetEnumerator();
        }

        public void Emmit(IJitWriter jitWriter)
        {
            jitWriter.WriteInstruction(OpCode.LoopStart);
            foreach (var instruction in Instructions)
            {
                instruction.Emmit(jitWriter);
            }
            jitWriter.WriteInstruction(OpCode.LoopEnd);
        }
    }
}
