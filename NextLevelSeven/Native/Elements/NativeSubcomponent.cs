﻿using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Represents a subcomponent-level element in an HL7 message.
    /// </summary>
    internal sealed class NativeSubcomponent : NativeElement, INativeSubcomponent
    {
        public NativeSubcomponent(NativeElement ancestor, int index, int externalIndex)
            : base(ancestor, index, externalIndex)
        {
        }

        private NativeSubcomponent(string value, EncodingConfiguration config)
            : base(value, config)
        {
        }

        public override char Delimiter
        {
            get { return '\0'; }
        }

        public override int ValueCount
        {
            get { return 0; }
        }

        public override bool HasSignificantDescendants
        {
            get { return false; }
        }

        public string GetValue()
        {
            return Value;
        }

        public IEnumerable<string> GetValues()
        {
            return Value.Yield();
        }

        public override IElement Clone()
        {
            return CloneInternal();
        }

        ISubcomponent ISubcomponent.Clone()
        {
            return CloneInternal();
        }

        public override INativeElement GetDescendant(int index)
        {
            throw new ElementException(ErrorCode.SubcomponentCannotHaveDescendants);
        }

        private NativeSubcomponent CloneInternal()
        {
            return new NativeSubcomponent(Value, EncodingConfiguration) {Index = Index};
        }
    }
}