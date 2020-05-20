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
    partial class CompilerTests_CSharp80_IndexRange
    {
        [TestMethod]
        public void CompilerTest_EDDE_8041()
        {
            // (Expression<Func<Index>>)(() => 1)
            var actual = GetDebugView(@"(Expression<Func<Index>>)(() => 1)");
            var expected = @"
<Lambda Type=""System.Func`1[System.Index]"">
  <Parameters />
  <Body>
    <Convert Type=""System.Index"" Method=""System.Index op_Implicit(Int32)"">
      <Operand>
        <Constant Type=""System.Int32"" Value=""1"" />
      </Operand>
    </Convert>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_EDDE_8041();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_EDDE_8041() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_F139_15B7()
        {
            // (Expression<Func<Index>>)(() => ^1)
            var actual = GetDebugView(@"(Expression<Func<Index>>)(() => ^1)");
            var expected = @"
<Lambda Type=""System.Func`1[System.Index]"">
  <Parameters />
  <Body>
    <CSharpFromEndIndex Type=""System.Index"" Method=""Void .ctor(Int32, Boolean)"">
      <Operand>
        <Constant Type=""System.Int32"" Value=""1"" />
      </Operand>
    </CSharpFromEndIndex>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_F139_15B7();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_F139_15B7() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_7E29_52A3()
        {
            // (Expression<Func<Range>>)(() => 1..2)
            var actual = GetDebugView(@"(Expression<Func<Range>>)(() => 1..2)");
            var expected = @"
<Lambda Type=""System.Func`1[System.Range]"">
  <Parameters />
  <Body>
    <CSharpRange Type=""System.Range"" Method=""Void .ctor(System.Index, System.Index)"">
      <Left>
        <Convert Type=""System.Index"" Method=""System.Index op_Implicit(Int32)"">
          <Operand>
            <Constant Type=""System.Int32"" Value=""1"" />
          </Operand>
        </Convert>
      </Left>
      <Right>
        <Convert Type=""System.Index"" Method=""System.Index op_Implicit(Int32)"">
          <Operand>
            <Constant Type=""System.Int32"" Value=""2"" />
          </Operand>
        </Convert>
      </Right>
    </CSharpRange>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_7E29_52A3();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_7E29_52A3() => INCONCLUSIVE(); }

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
    partial class CompilerTests_CSharp80_IndexRange
    {
        partial class Reviewed
        {
            public override void CompilerTest_EDDE_8041() => OK();
            public override void CompilerTest_F139_15B7() => OK();
            public override void CompilerTest_7E29_52A3() => OK();
        }
    }
}
*/
}
