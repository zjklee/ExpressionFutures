﻿// Prototyping extended expression trees for C#.
//
// bartde - November 2015

// NB: Running these tests can take a *VERY LONG* time because it invokes the C# compiler for every test
//     case in order to obtain an expression tree object. Be patient when running these tests.

// NB: These tests are generated from a list of expressions in the .tt file by invoking the C# compiler at
//     text template processing time by the T4 engine. See TestUtilities for the helper functions that call
//     into the compiler, load the generated assembly, extract the Expression objects through reflection on
//     the generated type, and call DebugView() on those. The resulting DebugView string is emitted in this
//     file as `expected` variables. The original expression is escaped and gets passed ot the GetDebugView
//     helper method to obtain `actual`, which causes the C# compiler to run at test execution time, using
//     the same helper library, thus obtaining the DebugView string again. This serves a number of goals:
//
//       1. At test generation time, a custom Roslyn build can be invoked to test the implicit conversion
//          of a lambda expression to an expression tree, which involves the changes made to support the
//          C# expression library in this solution. Any fatal compiler errors will come out at that time.
//
//       2. Reflection on the properties in the emitted class causes a deferred execution of the factory
//          method calls generated by the Roslyn compiler for the implicit conversion of the lambda to an
//          expression tree. Any exceptions thrown by the factory methods will show up as well during test
//          generation time, allowing issues to be uncovered.
//
//       3. The string literals in the `expected` variables are inspectable by a human to assert that the
//          compiler has indeed generated an expression representation that's homo-iconic to the original
//          expression that was provided in the test.
//
//       4. Any changes to the compiler or the runtime library could cause regressions. Because template
//          processing of the T4 only takes place upon editing the .tt file, the generated test code won't
//          change. As such, any regression can cause test failures which allows to detect any changes to
//          compiler or runtime library behavior.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Tests.Microsoft.CodeAnalysis.CSharp.TestUtilities;

namespace Tests.Microsoft.CodeAnalysis.CSharp
{
    [TestClass]
    public partial class CompilerTests_CSharp40_NamedAndOptionalParameters
    {
        [TestMethod]
        public void CompilerTest_E9F4_7C15()
        {
            // (Expression<Func<int>>)(() => Math.Abs(value: 42))
            var actual = GetDebugView(@"(Expression<Func<int>>)(() => Math.Abs(value: 42))");
            var expected = @"
<Lambda Type=""System.Func`1[System.Int32]"">
  <Parameters />
  <Body>
    <CSharpCall Type=""System.Int32"" Method=""Int32 Abs(Int32)"">
      <Arguments>
        <ParameterAssignment Parameter=""Int32 value"">
          <Expression>
            <Constant Type=""System.Int32"" Value=""42"" />
          </Expression>
        </ParameterAssignment>
      </Arguments>
    </CSharpCall>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_E9F4_7C15();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_E9F4_7C15() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_4EB1_83FD()
        {
            // (Expression<Func<string, string>>)(s => s.Substring(startIndex: 42))
            var actual = GetDebugView(@"(Expression<Func<string, string>>)(s => s.Substring(startIndex: 42))");
            var expected = @"
<Lambda Type=""System.Func`2[System.String,System.String]"">
  <Parameters>
    <Parameter Type=""System.String"" Id=""0"" Name=""s"" />
  </Parameters>
  <Body>
    <CSharpCall Type=""System.String"" Method=""System.String Substring(Int32)"">
      <Object>
        <Parameter Type=""System.String"" Id=""0"" Name=""s"" />
      </Object>
      <Arguments>
        <ParameterAssignment Parameter=""Int32 startIndex"">
          <Expression>
            <Constant Type=""System.Int32"" Value=""42"" />
          </Expression>
        </ParameterAssignment>
      </Arguments>
    </CSharpCall>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_4EB1_83FD();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_4EB1_83FD() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_C437_AA4C()
        {
            // (Expression<Func<string, string>>)(s => s.Substring(startIndex: 42, length: 43))
            var actual = GetDebugView(@"(Expression<Func<string, string>>)(s => s.Substring(startIndex: 42, length: 43))");
            var expected = @"
<Lambda Type=""System.Func`2[System.String,System.String]"">
  <Parameters>
    <Parameter Type=""System.String"" Id=""0"" Name=""s"" />
  </Parameters>
  <Body>
    <CSharpCall Type=""System.String"" Method=""System.String Substring(Int32, Int32)"">
      <Object>
        <Parameter Type=""System.String"" Id=""0"" Name=""s"" />
      </Object>
      <Arguments>
        <ParameterAssignment Parameter=""Int32 startIndex"">
          <Expression>
            <Constant Type=""System.Int32"" Value=""42"" />
          </Expression>
        </ParameterAssignment>
        <ParameterAssignment Parameter=""Int32 length"">
          <Expression>
            <Constant Type=""System.Int32"" Value=""43"" />
          </Expression>
        </ParameterAssignment>
      </Arguments>
    </CSharpCall>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_C437_AA4C();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_C437_AA4C() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_4C39_BCFC()
        {
            // (Expression<Func<string, string>>)(s => s.Substring(length: 43, startIndex: 42))
            var actual = GetDebugView(@"(Expression<Func<string, string>>)(s => s.Substring(length: 43, startIndex: 42))");
            var expected = @"
<Lambda Type=""System.Func`2[System.String,System.String]"">
  <Parameters>
    <Parameter Type=""System.String"" Id=""0"" Name=""s"" />
  </Parameters>
  <Body>
    <CSharpCall Type=""System.String"" Method=""System.String Substring(Int32, Int32)"">
      <Object>
        <Parameter Type=""System.String"" Id=""0"" Name=""s"" />
      </Object>
      <Arguments>
        <ParameterAssignment Parameter=""Int32 length"">
          <Expression>
            <Constant Type=""System.Int32"" Value=""43"" />
          </Expression>
        </ParameterAssignment>
        <ParameterAssignment Parameter=""Int32 startIndex"">
          <Expression>
            <Constant Type=""System.Int32"" Value=""42"" />
          </Expression>
        </ParameterAssignment>
      </Arguments>
    </CSharpCall>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_4C39_BCFC();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_4C39_BCFC() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_7E8C_AA4C()
        {
            // (Expression<Func<string, string>>)(s => s.Substring(42, length: 43))
            var actual = GetDebugView(@"(Expression<Func<string, string>>)(s => s.Substring(42, length: 43))");
            var expected = @"
<Lambda Type=""System.Func`2[System.String,System.String]"">
  <Parameters>
    <Parameter Type=""System.String"" Id=""0"" Name=""s"" />
  </Parameters>
  <Body>
    <CSharpCall Type=""System.String"" Method=""System.String Substring(Int32, Int32)"">
      <Object>
        <Parameter Type=""System.String"" Id=""0"" Name=""s"" />
      </Object>
      <Arguments>
        <ParameterAssignment Parameter=""Int32 startIndex"">
          <Expression>
            <Constant Type=""System.Int32"" Value=""42"" />
          </Expression>
        </ParameterAssignment>
        <ParameterAssignment Parameter=""Int32 length"">
          <Expression>
            <Constant Type=""System.Int32"" Value=""43"" />
          </Expression>
        </ParameterAssignment>
      </Arguments>
    </CSharpCall>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_7E8C_AA4C();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_7E8C_AA4C() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_00C1_AE5C()
        {
            // (Expression<Func<TimeSpan>>)(() => new TimeSpan(ticks: 42L))
            var actual = GetDebugView(@"(Expression<Func<TimeSpan>>)(() => new TimeSpan(ticks: 42L))");
            var expected = @"
<Lambda Type=""System.Func`1[System.TimeSpan]"">
  <Parameters />
  <Body>
    <CSharpNew Type=""System.TimeSpan"" Constructor=""Void .ctor(Int64)"">
      <Arguments>
        <ParameterAssignment Parameter=""Int64 ticks"">
          <Expression>
            <Constant Type=""System.Int64"" Value=""42"" />
          </Expression>
        </ParameterAssignment>
      </Arguments>
    </CSharpNew>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_00C1_AE5C();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_00C1_AE5C() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_D9CA_6B19()
        {
            // (Expression<Func<TimeSpan>>)(() => new TimeSpan(seconds: 3, minutes: 2, hours: 1))
            var actual = GetDebugView(@"(Expression<Func<TimeSpan>>)(() => new TimeSpan(seconds: 3, minutes: 2, hours: 1))");
            var expected = @"
<Lambda Type=""System.Func`1[System.TimeSpan]"">
  <Parameters />
  <Body>
    <CSharpNew Type=""System.TimeSpan"" Constructor=""Void .ctor(Int32, Int32, Int32)"">
      <Arguments>
        <ParameterAssignment Parameter=""Int32 seconds"">
          <Expression>
            <Constant Type=""System.Int32"" Value=""3"" />
          </Expression>
        </ParameterAssignment>
        <ParameterAssignment Parameter=""Int32 minutes"">
          <Expression>
            <Constant Type=""System.Int32"" Value=""2"" />
          </Expression>
        </ParameterAssignment>
        <ParameterAssignment Parameter=""Int32 hours"">
          <Expression>
            <Constant Type=""System.Int32"" Value=""1"" />
          </Expression>
        </ParameterAssignment>
      </Arguments>
    </CSharpNew>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_D9CA_6B19();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_D9CA_6B19() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_EDEC_D0C9()
        {
            // (Expression<Func<List<int>, int>>)(xs => xs[index: 42])
            var actual = GetDebugView(@"(Expression<Func<List<int>, int>>)(xs => xs[index: 42])");
            var expected = @"
<Lambda Type=""System.Func`2[System.Collections.Generic.List`1[System.Int32],System.Int32]"">
  <Parameters>
    <Parameter Type=""System.Collections.Generic.List`1[System.Int32]"" Id=""0"" Name=""xs"" />
  </Parameters>
  <Body>
    <CSharpIndex Type=""System.Int32"" Indexer=""Int32 Item [Int32]"">
      <Object>
        <Parameter Type=""System.Collections.Generic.List`1[System.Int32]"" Id=""0"" Name=""xs"" />
      </Object>
      <Arguments>
        <ParameterAssignment Parameter=""Int32 index"">
          <Expression>
            <Constant Type=""System.Int32"" Value=""42"" />
          </Expression>
        </ParameterAssignment>
      </Arguments>
    </CSharpIndex>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_EDEC_D0C9();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_EDEC_D0C9() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_6271_EABC()
        {
            // (Expression<Action<Action<int>>>)(a => a(obj: 42))
            var actual = GetDebugView(@"(Expression<Action<Action<int>>>)(a => a(obj: 42))");
            var expected = @"
<Lambda Type=""System.Action`1[System.Action`1[System.Int32]]"">
  <Parameters>
    <Parameter Type=""System.Action`1[System.Int32]"" Id=""0"" Name=""a"" />
  </Parameters>
  <Body>
    <CSharpInvoke Type=""System.Void"">
      <Expression>
        <Parameter Type=""System.Action`1[System.Int32]"" Id=""0"" Name=""a"" />
      </Expression>
      <Arguments>
        <ParameterAssignment Parameter=""Int32 obj"">
          <Expression>
            <Constant Type=""System.Int32"" Value=""42"" />
          </Expression>
        </ParameterAssignment>
      </Arguments>
    </CSharpInvoke>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_6271_EABC();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_6271_EABC() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_053A_671C()
        {
            // (Expression<Action<Action<string, int, bool>>>)(a => a(arg2: 42, arg1: "foo", arg3: false))
            var actual = GetDebugView(@"(Expression<Action<Action<string, int, bool>>>)(a => a(arg2: 42, arg1: ""foo"", arg3: false))");
            var expected = @"
<Lambda Type=""System.Action`1[System.Action`3[System.String,System.Int32,System.Boolean]]"">
  <Parameters>
    <Parameter Type=""System.Action`3[System.String,System.Int32,System.Boolean]"" Id=""0"" Name=""a"" />
  </Parameters>
  <Body>
    <CSharpInvoke Type=""System.Void"">
      <Expression>
        <Parameter Type=""System.Action`3[System.String,System.Int32,System.Boolean]"" Id=""0"" Name=""a"" />
      </Expression>
      <Arguments>
        <ParameterAssignment Parameter=""Int32 arg2"">
          <Expression>
            <Constant Type=""System.Int32"" Value=""42"" />
          </Expression>
        </ParameterAssignment>
        <ParameterAssignment Parameter=""System.String arg1"">
          <Expression>
            <Constant Type=""System.String"" Value=""foo"" />
          </Expression>
        </ParameterAssignment>
        <ParameterAssignment Parameter=""Boolean arg3"">
          <Expression>
            <Constant Type=""System.Boolean"" Value=""false"" />
          </Expression>
        </ParameterAssignment>
      </Arguments>
    </CSharpInvoke>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_053A_671C();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_053A_671C() => INCONCLUSIVE(); }

        partial class Review
        {
            protected void INCONCLUSIVE() { Assert.Inconclusive(); }
        }

        partial class Reviewed : Review
        {
            private void OK() { }
            private void FAIL(string message = "") { Assert.Fail(message); }
        }

        private readonly Reviewed Verify = new Reviewed();
    }

/*
// NB: The code generated below accepts all tests. *DON'T* just copy/paste this to the .Verify.cs file
//     but review the tests one by one. This output is included in case a minor change is made to debug
//     output produced by DebugView() and all hashes are invalidated. In that case, this output can be
//     copied and pasted into .Verify.cs.

namespace Tests.Microsoft.CodeAnalysis.CSharp
{
    partial class CompilerTests_CSharp40_NamedAndOptionalParameters
    {
        partial class Reviewed
        {
            public override void CompilerTest_E9F4_7C15() => OK();
            public override void CompilerTest_4EB1_83FD() => OK();
            public override void CompilerTest_C437_AA4C() => OK();
            public override void CompilerTest_4C39_BCFC() => OK();
            public override void CompilerTest_7E8C_AA4C() => OK();
            public override void CompilerTest_00C1_AE5C() => OK();
            public override void CompilerTest_D9CA_6B19() => OK();
            public override void CompilerTest_EDEC_D0C9() => OK();
            public override void CompilerTest_6271_EABC() => OK();
            public override void CompilerTest_053A_671C() => OK();
        }
    }
}
*/
}
