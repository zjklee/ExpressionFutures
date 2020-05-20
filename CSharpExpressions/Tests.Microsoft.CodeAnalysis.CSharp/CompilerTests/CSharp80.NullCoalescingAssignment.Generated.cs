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
    public partial class CompilerTests_CSharp80_NullCoalescingAssignment
    {
        [TestMethod]
        public void CompilerTest_A465_3AAF()
        {
            // (Expression<Func<string, string>>)(s => s ??= "foo")
            var actual = GetDebugView(@"(Expression<Func<string, string>>)(s => s ??= ""foo"")");
            var expected = @"
<Lambda Type=""System.Func`2[System.String,System.String]"">
  <Parameters>
    <Parameter Type=""System.String"" Id=""0"" Name=""s"" />
  </Parameters>
  <Body>
    <CSharpNullCoalescingAssign Type=""System.String"">
      <Left>
        <Parameter Type=""System.String"" Id=""0"" Name=""s"" />
      </Left>
      <Right>
        <Constant Type=""System.String"" Value=""foo"" />
      </Right>
    </CSharpNullCoalescingAssign>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_A465_3AAF();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_A465_3AAF() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_07E0_24A3()
        {
            // (Expression<Func<int?, int?, int?>>)((i, j) => i ??= j)
            var actual = GetDebugView(@"(Expression<Func<int?, int?, int?>>)((i, j) => i ??= j)");
            var expected = @"
<Lambda Type=""System.Func`3[System.Nullable`1[System.Int32],System.Nullable`1[System.Int32],System.Nullable`1[System.Int32]]"">
  <Parameters>
    <Parameter Type=""System.Nullable`1[System.Int32]"" Id=""0"" Name=""i"" />
    <Parameter Type=""System.Nullable`1[System.Int32]"" Id=""1"" Name=""j"" />
  </Parameters>
  <Body>
    <CSharpNullCoalescingAssign Type=""System.Nullable`1[System.Int32]"">
      <Left>
        <Parameter Type=""System.Nullable`1[System.Int32]"" Id=""0"" Name=""i"" />
      </Left>
      <Right>
        <Parameter Type=""System.Nullable`1[System.Int32]"" Id=""1"" Name=""j"" />
      </Right>
    </CSharpNullCoalescingAssign>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_07E0_24A3();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_07E0_24A3() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_22AE_900C()
        {
            // (Expression<Func<int?, int>>)(i => i ??= 42)
            var actual = GetDebugView(@"(Expression<Func<int?, int>>)(i => i ??= 42)");
            var expected = @"
<Lambda Type=""System.Func`2[System.Nullable`1[System.Int32],System.Int32]"">
  <Parameters>
    <Parameter Type=""System.Nullable`1[System.Int32]"" Id=""0"" Name=""i"" />
  </Parameters>
  <Body>
    <CSharpNullCoalescingAssign Type=""System.Int32"">
      <Left>
        <Parameter Type=""System.Nullable`1[System.Int32]"" Id=""0"" Name=""i"" />
      </Left>
      <Right>
        <Constant Type=""System.Int32"" Value=""42"" />
      </Right>
    </CSharpNullCoalescingAssign>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_22AE_900C();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_22AE_900C() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_0FE1_86B6()
        {
            // (Expression<Func<string, dynamic, string>>)((s, d) => s ??= d.bar)
            var actual = GetDebugView(@"(Expression<Func<string, dynamic, string>>)((s, d) => s ??= d.bar)");
            var expected = @"
<Lambda Type=""System.Func`3[System.String,System.Object,System.String]"">
  <Parameters>
    <Parameter Type=""System.String"" Id=""0"" Name=""s"" />
    <Parameter Type=""System.Object"" Id=""1"" Name=""d"" />
  </Parameters>
  <Body>
    <CSharpNullCoalescingAssign Type=""System.String"">
      <Left>
        <Parameter Type=""System.String"" Id=""0"" Name=""s"" />
      </Left>
      <Right>
        <CSharpDynamicConvert Type=""System.String"" Context=""Expressions"">
          <Expression>
            <CSharpDynamicGetMember Type=""System.Object"" Name=""bar"" Context=""Expressions"">
              <Object>
                <Parameter Type=""System.Object"" Id=""1"" Name=""d"" />
              </Object>
            </CSharpDynamicGetMember>
          </Expression>
        </CSharpDynamicConvert>
      </Right>
    </CSharpNullCoalescingAssign>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_0FE1_86B6();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_0FE1_86B6() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_75E3_213A()
        {
            // (Expression<Func<string[], dynamic, string>>)((ss, d) => ss[int.Parse("0")] ??= d.bar)
            var actual = GetDebugView(@"(Expression<Func<string[], dynamic, string>>)((ss, d) => ss[int.Parse(""0"")] ??= d.bar)");
            var expected = @"
<Lambda Type=""System.Func`3[System.String[],System.Object,System.String]"">
  <Parameters>
    <Parameter Type=""System.String[]"" Id=""0"" Name=""ss"" />
    <Parameter Type=""System.Object"" Id=""1"" Name=""d"" />
  </Parameters>
  <Body>
    <CSharpNullCoalescingAssign Type=""System.String"">
      <Left>
        <ArrayIndex Type=""System.String"">
          <Left>
            <Parameter Type=""System.String[]"" Id=""0"" Name=""ss"" />
          </Left>
          <Right>
            <Call Type=""System.Int32"" Method=""Int32 Parse(System.String)"">
              <Arguments>
                <Constant Type=""System.String"" Value=""0"" />
              </Arguments>
            </Call>
          </Right>
        </ArrayIndex>
      </Left>
      <Right>
        <CSharpDynamicConvert Type=""System.String"" Context=""Expressions"">
          <Expression>
            <CSharpDynamicGetMember Type=""System.Object"" Name=""bar"" Context=""Expressions"">
              <Object>
                <Parameter Type=""System.Object"" Id=""1"" Name=""d"" />
              </Object>
            </CSharpDynamicGetMember>
          </Expression>
        </CSharpDynamicConvert>
      </Right>
    </CSharpNullCoalescingAssign>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_75E3_213A();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_75E3_213A() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_9CD5_03D0()
        {
            // (Expression<Func<List<string>, dynamic, string>>)((ss, d) => ss[int.Parse("0")] ??= d.bar)
            var actual = GetDebugView(@"(Expression<Func<List<string>, dynamic, string>>)((ss, d) => ss[int.Parse(""0"")] ??= d.bar)");
            var expected = @"
<Lambda Type=""System.Func`3[System.Collections.Generic.List`1[System.String],System.Object,System.String]"">
  <Parameters>
    <Parameter Type=""System.Collections.Generic.List`1[System.String]"" Id=""0"" Name=""ss"" />
    <Parameter Type=""System.Object"" Id=""1"" Name=""d"" />
  </Parameters>
  <Body>
    <CSharpNullCoalescingAssign Type=""System.String"">
      <Left>
        <CSharpIndex Type=""System.String"" Indexer=""System.String Item [Int32]"">
          <Object>
            <Parameter Type=""System.Collections.Generic.List`1[System.String]"" Id=""0"" Name=""ss"" />
          </Object>
          <Arguments>
            <ParameterAssignment Parameter=""Int32 index"">
              <Expression>
                <Call Type=""System.Int32"" Method=""Int32 Parse(System.String)"">
                  <Arguments>
                    <Constant Type=""System.String"" Value=""0"" />
                  </Arguments>
                </Call>
              </Expression>
            </ParameterAssignment>
          </Arguments>
        </CSharpIndex>
      </Left>
      <Right>
        <CSharpDynamicConvert Type=""System.String"" Context=""Expressions"">
          <Expression>
            <CSharpDynamicGetMember Type=""System.Object"" Name=""bar"" Context=""Expressions"">
              <Object>
                <Parameter Type=""System.Object"" Id=""1"" Name=""d"" />
              </Object>
            </CSharpDynamicGetMember>
          </Expression>
        </CSharpDynamicConvert>
      </Right>
    </CSharpNullCoalescingAssign>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_9CD5_03D0();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_9CD5_03D0() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_330E_2E05()
        {
            // (Expression<Func<StrongBox<string>, dynamic, string>>)((s, d) => s.Value ??= d.bar)
            var actual = GetDebugView(@"(Expression<Func<StrongBox<string>, dynamic, string>>)((s, d) => s.Value ??= d.bar)");
            var expected = @"
<Lambda Type=""System.Func`3[System.Runtime.CompilerServices.StrongBox`1[System.String],System.Object,System.String]"">
  <Parameters>
    <Parameter Type=""System.Runtime.CompilerServices.StrongBox`1[System.String]"" Id=""0"" Name=""s"" />
    <Parameter Type=""System.Object"" Id=""1"" Name=""d"" />
  </Parameters>
  <Body>
    <CSharpNullCoalescingAssign Type=""System.String"">
      <Left>
        <MemberAccess Type=""System.String"" Member=""System.String Value"">
          <Expression>
            <Parameter Type=""System.Runtime.CompilerServices.StrongBox`1[System.String]"" Id=""0"" Name=""s"" />
          </Expression>
        </MemberAccess>
      </Left>
      <Right>
        <CSharpDynamicConvert Type=""System.String"" Context=""Expressions"">
          <Expression>
            <CSharpDynamicGetMember Type=""System.Object"" Name=""bar"" Context=""Expressions"">
              <Object>
                <Parameter Type=""System.Object"" Id=""1"" Name=""d"" />
              </Object>
            </CSharpDynamicGetMember>
          </Expression>
        </CSharpDynamicConvert>
      </Right>
    </CSharpNullCoalescingAssign>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_330E_2E05();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_330E_2E05() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_B81A_515D()
        {
            // (Expression<Func<string, dynamic, string>>)((s, d) => s ??= d.foo[int.Parse("1")])
            var actual = GetDebugView(@"(Expression<Func<string, dynamic, string>>)((s, d) => s ??= d.foo[int.Parse(""1"")])");
            var expected = @"
<Lambda Type=""System.Func`3[System.String,System.Object,System.String]"">
  <Parameters>
    <Parameter Type=""System.String"" Id=""0"" Name=""s"" />
    <Parameter Type=""System.Object"" Id=""1"" Name=""d"" />
  </Parameters>
  <Body>
    <CSharpNullCoalescingAssign Type=""System.String"">
      <Left>
        <Parameter Type=""System.String"" Id=""0"" Name=""s"" />
      </Left>
      <Right>
        <CSharpDynamicConvert Type=""System.String"" Context=""Expressions"">
          <Expression>
            <CSharpDynamicGetIndex Type=""System.Object"" Context=""Expressions"">
              <Object>
                <CSharpDynamicGetMember Type=""System.Object"" Name=""foo"" Flags=""ResultIndexed"" Context=""Expressions"">
                  <Object>
                    <Parameter Type=""System.Object"" Id=""1"" Name=""d"" />
                  </Object>
                </CSharpDynamicGetMember>
              </Object>
              <Arguments>
                <DynamicCSharpArgument Flags=""UseCompileTimeType"">
                  <Expression>
                    <Call Type=""System.Int32"" Method=""Int32 Parse(System.String)"">
                      <Arguments>
                        <Constant Type=""System.String"" Value=""1"" />
                      </Arguments>
                    </Call>
                  </Expression>
                </DynamicCSharpArgument>
              </Arguments>
            </CSharpDynamicGetIndex>
          </Expression>
        </CSharpDynamicConvert>
      </Right>
    </CSharpNullCoalescingAssign>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_B81A_515D();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_B81A_515D() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_7AF5_0A0E()
        {
            // (Expression<Func<string[], dynamic, string>>)((ss, d) => ss[int.Parse("0")] ??= d.foo[int.Parse("1")])
            var actual = GetDebugView(@"(Expression<Func<string[], dynamic, string>>)((ss, d) => ss[int.Parse(""0"")] ??= d.foo[int.Parse(""1"")])");
            var expected = @"
<Lambda Type=""System.Func`3[System.String[],System.Object,System.String]"">
  <Parameters>
    <Parameter Type=""System.String[]"" Id=""0"" Name=""ss"" />
    <Parameter Type=""System.Object"" Id=""1"" Name=""d"" />
  </Parameters>
  <Body>
    <CSharpNullCoalescingAssign Type=""System.String"">
      <Left>
        <ArrayIndex Type=""System.String"">
          <Left>
            <Parameter Type=""System.String[]"" Id=""0"" Name=""ss"" />
          </Left>
          <Right>
            <Call Type=""System.Int32"" Method=""Int32 Parse(System.String)"">
              <Arguments>
                <Constant Type=""System.String"" Value=""0"" />
              </Arguments>
            </Call>
          </Right>
        </ArrayIndex>
      </Left>
      <Right>
        <CSharpDynamicConvert Type=""System.String"" Context=""Expressions"">
          <Expression>
            <CSharpDynamicGetIndex Type=""System.Object"" Context=""Expressions"">
              <Object>
                <CSharpDynamicGetMember Type=""System.Object"" Name=""foo"" Flags=""ResultIndexed"" Context=""Expressions"">
                  <Object>
                    <Parameter Type=""System.Object"" Id=""1"" Name=""d"" />
                  </Object>
                </CSharpDynamicGetMember>
              </Object>
              <Arguments>
                <DynamicCSharpArgument Flags=""UseCompileTimeType"">
                  <Expression>
                    <Call Type=""System.Int32"" Method=""Int32 Parse(System.String)"">
                      <Arguments>
                        <Constant Type=""System.String"" Value=""1"" />
                      </Arguments>
                    </Call>
                  </Expression>
                </DynamicCSharpArgument>
              </Arguments>
            </CSharpDynamicGetIndex>
          </Expression>
        </CSharpDynamicConvert>
      </Right>
    </CSharpNullCoalescingAssign>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_7AF5_0A0E();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_7AF5_0A0E() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_50DE_079F()
        {
            // (Expression<Func<List<string>, dynamic, string>>)((ss, d) => ss[int.Parse("0")] ??= d.foo[int.Parse("1")])
            var actual = GetDebugView(@"(Expression<Func<List<string>, dynamic, string>>)((ss, d) => ss[int.Parse(""0"")] ??= d.foo[int.Parse(""1"")])");
            var expected = @"
<Lambda Type=""System.Func`3[System.Collections.Generic.List`1[System.String],System.Object,System.String]"">
  <Parameters>
    <Parameter Type=""System.Collections.Generic.List`1[System.String]"" Id=""0"" Name=""ss"" />
    <Parameter Type=""System.Object"" Id=""1"" Name=""d"" />
  </Parameters>
  <Body>
    <CSharpNullCoalescingAssign Type=""System.String"">
      <Left>
        <CSharpIndex Type=""System.String"" Indexer=""System.String Item [Int32]"">
          <Object>
            <Parameter Type=""System.Collections.Generic.List`1[System.String]"" Id=""0"" Name=""ss"" />
          </Object>
          <Arguments>
            <ParameterAssignment Parameter=""Int32 index"">
              <Expression>
                <Call Type=""System.Int32"" Method=""Int32 Parse(System.String)"">
                  <Arguments>
                    <Constant Type=""System.String"" Value=""0"" />
                  </Arguments>
                </Call>
              </Expression>
            </ParameterAssignment>
          </Arguments>
        </CSharpIndex>
      </Left>
      <Right>
        <CSharpDynamicConvert Type=""System.String"" Context=""Expressions"">
          <Expression>
            <CSharpDynamicGetIndex Type=""System.Object"" Context=""Expressions"">
              <Object>
                <CSharpDynamicGetMember Type=""System.Object"" Name=""foo"" Flags=""ResultIndexed"" Context=""Expressions"">
                  <Object>
                    <Parameter Type=""System.Object"" Id=""1"" Name=""d"" />
                  </Object>
                </CSharpDynamicGetMember>
              </Object>
              <Arguments>
                <DynamicCSharpArgument Flags=""UseCompileTimeType"">
                  <Expression>
                    <Call Type=""System.Int32"" Method=""Int32 Parse(System.String)"">
                      <Arguments>
                        <Constant Type=""System.String"" Value=""1"" />
                      </Arguments>
                    </Call>
                  </Expression>
                </DynamicCSharpArgument>
              </Arguments>
            </CSharpDynamicGetIndex>
          </Expression>
        </CSharpDynamicConvert>
      </Right>
    </CSharpNullCoalescingAssign>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_50DE_079F();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_50DE_079F() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_FE8E_B94B()
        {
            // (Expression<Func<StrongBox<string>, dynamic, string>>)((s, d) => s.Value ??= d.foo[int.Parse("1")])
            var actual = GetDebugView(@"(Expression<Func<StrongBox<string>, dynamic, string>>)((s, d) => s.Value ??= d.foo[int.Parse(""1"")])");
            var expected = @"
<Lambda Type=""System.Func`3[System.Runtime.CompilerServices.StrongBox`1[System.String],System.Object,System.String]"">
  <Parameters>
    <Parameter Type=""System.Runtime.CompilerServices.StrongBox`1[System.String]"" Id=""0"" Name=""s"" />
    <Parameter Type=""System.Object"" Id=""1"" Name=""d"" />
  </Parameters>
  <Body>
    <CSharpNullCoalescingAssign Type=""System.String"">
      <Left>
        <MemberAccess Type=""System.String"" Member=""System.String Value"">
          <Expression>
            <Parameter Type=""System.Runtime.CompilerServices.StrongBox`1[System.String]"" Id=""0"" Name=""s"" />
          </Expression>
        </MemberAccess>
      </Left>
      <Right>
        <CSharpDynamicConvert Type=""System.String"" Context=""Expressions"">
          <Expression>
            <CSharpDynamicGetIndex Type=""System.Object"" Context=""Expressions"">
              <Object>
                <CSharpDynamicGetMember Type=""System.Object"" Name=""foo"" Flags=""ResultIndexed"" Context=""Expressions"">
                  <Object>
                    <Parameter Type=""System.Object"" Id=""1"" Name=""d"" />
                  </Object>
                </CSharpDynamicGetMember>
              </Object>
              <Arguments>
                <DynamicCSharpArgument Flags=""UseCompileTimeType"">
                  <Expression>
                    <Call Type=""System.Int32"" Method=""Int32 Parse(System.String)"">
                      <Arguments>
                        <Constant Type=""System.String"" Value=""1"" />
                      </Arguments>
                    </Call>
                  </Expression>
                </DynamicCSharpArgument>
              </Arguments>
            </CSharpDynamicGetIndex>
          </Expression>
        </CSharpDynamicConvert>
      </Right>
    </CSharpNullCoalescingAssign>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_FE8E_B94B();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_FE8E_B94B() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_9440_85D5()
        {
            // (Expression<Func<dynamic, string, string>>)((d, s) => d.bar ??= s)
            var actual = GetDebugView(@"(Expression<Func<dynamic, string, string>>)((d, s) => d.bar ??= s)");
            var expected = @"
<Lambda Type=""System.Func`3[System.Object,System.String,System.String]"">
  <Parameters>
    <Parameter Type=""System.Object"" Id=""0"" Name=""d"" />
    <Parameter Type=""System.String"" Id=""1"" Name=""s"" />
  </Parameters>
  <Body>
    <CSharpDynamicConvert Type=""System.String"" Context=""Expressions"">
      <Expression>
        <CSharpDynamicBinaryAssign Type=""System.Object"" OperationNodeType=""NullCoalescingAssign"">
          <Left>
            <DynamicCSharpArgument>
              <Expression>
                <CSharpDynamicGetMember Type=""System.Object"" Name=""bar"" Context=""Expressions"">
                  <Object>
                    <Parameter Type=""System.Object"" Id=""0"" Name=""d"" />
                  </Object>
                </CSharpDynamicGetMember>
              </Expression>
            </DynamicCSharpArgument>
          </Left>
          <Right>
            <DynamicCSharpArgument Flags=""UseCompileTimeType"">
              <Expression>
                <Parameter Type=""System.String"" Id=""1"" Name=""s"" />
              </Expression>
            </DynamicCSharpArgument>
          </Right>
        </CSharpDynamicBinaryAssign>
      </Expression>
    </CSharpDynamicConvert>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_9440_85D5();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_9440_85D5() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_2C1D_9A84()
        {
            // (Expression<Func<dynamic, int, int>>)((d, x) => d.bar ??= x)
            var actual = GetDebugView(@"(Expression<Func<dynamic, int, int>>)((d, x) => d.bar ??= x)");
            var expected = @"
<Lambda Type=""System.Func`3[System.Object,System.Int32,System.Int32]"">
  <Parameters>
    <Parameter Type=""System.Object"" Id=""0"" Name=""d"" />
    <Parameter Type=""System.Int32"" Id=""1"" Name=""x"" />
  </Parameters>
  <Body>
    <CSharpDynamicConvert Type=""System.Int32"" Context=""Expressions"">
      <Expression>
        <CSharpDynamicBinaryAssign Type=""System.Object"" OperationNodeType=""NullCoalescingAssign"">
          <Left>
            <DynamicCSharpArgument>
              <Expression>
                <CSharpDynamicGetMember Type=""System.Object"" Name=""bar"" Context=""Expressions"">
                  <Object>
                    <Parameter Type=""System.Object"" Id=""0"" Name=""d"" />
                  </Object>
                </CSharpDynamicGetMember>
              </Expression>
            </DynamicCSharpArgument>
          </Left>
          <Right>
            <DynamicCSharpArgument>
              <Expression>
                <Convert Type=""System.Object"">
                  <Operand>
                    <Parameter Type=""System.Int32"" Id=""1"" Name=""x"" />
                  </Operand>
                </Convert>
              </Expression>
            </DynamicCSharpArgument>
          </Right>
        </CSharpDynamicBinaryAssign>
      </Expression>
    </CSharpDynamicConvert>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_2C1D_9A84();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_2C1D_9A84() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_F57B_8C67()
        {
            // (Expression<Func<dynamic, string, string>>)((d, s) => d[int.Parse("0")] ??= s)
            var actual = GetDebugView(@"(Expression<Func<dynamic, string, string>>)((d, s) => d[int.Parse(""0"")] ??= s)");
            var expected = @"
<Lambda Type=""System.Func`3[System.Object,System.String,System.String]"">
  <Parameters>
    <Parameter Type=""System.Object"" Id=""0"" Name=""d"" />
    <Parameter Type=""System.String"" Id=""1"" Name=""s"" />
  </Parameters>
  <Body>
    <CSharpDynamicConvert Type=""System.String"" Context=""Expressions"">
      <Expression>
        <CSharpDynamicBinaryAssign Type=""System.Object"" OperationNodeType=""NullCoalescingAssign"">
          <Left>
            <DynamicCSharpArgument>
              <Expression>
                <CSharpDynamicGetIndex Type=""System.Object"" Context=""Expressions"">
                  <Object>
                    <Parameter Type=""System.Object"" Id=""0"" Name=""d"" />
                  </Object>
                  <Arguments>
                    <DynamicCSharpArgument Flags=""UseCompileTimeType"">
                      <Expression>
                        <Call Type=""System.Int32"" Method=""Int32 Parse(System.String)"">
                          <Arguments>
                            <Constant Type=""System.String"" Value=""0"" />
                          </Arguments>
                        </Call>
                      </Expression>
                    </DynamicCSharpArgument>
                  </Arguments>
                </CSharpDynamicGetIndex>
              </Expression>
            </DynamicCSharpArgument>
          </Left>
          <Right>
            <DynamicCSharpArgument Flags=""UseCompileTimeType"">
              <Expression>
                <Parameter Type=""System.String"" Id=""1"" Name=""s"" />
              </Expression>
            </DynamicCSharpArgument>
          </Right>
        </CSharpDynamicBinaryAssign>
      </Expression>
    </CSharpDynamicConvert>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_F57B_8C67();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_F57B_8C67() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_734B_65DC()
        {
            // (Expression<Func<dynamic, int, int>>)((d, x) => d[int.Parse("0")] ??= x)
            var actual = GetDebugView(@"(Expression<Func<dynamic, int, int>>)((d, x) => d[int.Parse(""0"")] ??= x)");
            var expected = @"
<Lambda Type=""System.Func`3[System.Object,System.Int32,System.Int32]"">
  <Parameters>
    <Parameter Type=""System.Object"" Id=""0"" Name=""d"" />
    <Parameter Type=""System.Int32"" Id=""1"" Name=""x"" />
  </Parameters>
  <Body>
    <CSharpDynamicConvert Type=""System.Int32"" Context=""Expressions"">
      <Expression>
        <CSharpDynamicBinaryAssign Type=""System.Object"" OperationNodeType=""NullCoalescingAssign"">
          <Left>
            <DynamicCSharpArgument>
              <Expression>
                <CSharpDynamicGetIndex Type=""System.Object"" Context=""Expressions"">
                  <Object>
                    <Parameter Type=""System.Object"" Id=""0"" Name=""d"" />
                  </Object>
                  <Arguments>
                    <DynamicCSharpArgument Flags=""UseCompileTimeType"">
                      <Expression>
                        <Call Type=""System.Int32"" Method=""Int32 Parse(System.String)"">
                          <Arguments>
                            <Constant Type=""System.String"" Value=""0"" />
                          </Arguments>
                        </Call>
                      </Expression>
                    </DynamicCSharpArgument>
                  </Arguments>
                </CSharpDynamicGetIndex>
              </Expression>
            </DynamicCSharpArgument>
          </Left>
          <Right>
            <DynamicCSharpArgument>
              <Expression>
                <Convert Type=""System.Object"">
                  <Operand>
                    <Parameter Type=""System.Int32"" Id=""1"" Name=""x"" />
                  </Operand>
                </Convert>
              </Expression>
            </DynamicCSharpArgument>
          </Right>
        </CSharpDynamicBinaryAssign>
      </Expression>
    </CSharpDynamicConvert>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_734B_65DC();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_734B_65DC() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_BBC0_6EA4()
        {
            // (Expression<Func<dynamic, dynamic>>)(d => d.bar ??= d.foo)
            var actual = GetDebugView(@"(Expression<Func<dynamic, dynamic>>)(d => d.bar ??= d.foo)");
            var expected = @"
<Lambda Type=""System.Func`2[System.Object,System.Object]"">
  <Parameters>
    <Parameter Type=""System.Object"" Id=""0"" Name=""d"" />
  </Parameters>
  <Body>
    <CSharpDynamicBinaryAssign Type=""System.Object"" OperationNodeType=""NullCoalescingAssign"">
      <Left>
        <DynamicCSharpArgument>
          <Expression>
            <CSharpDynamicGetMember Type=""System.Object"" Name=""bar"" Context=""Expressions"">
              <Object>
                <Parameter Type=""System.Object"" Id=""0"" Name=""d"" />
              </Object>
            </CSharpDynamicGetMember>
          </Expression>
        </DynamicCSharpArgument>
      </Left>
      <Right>
        <DynamicCSharpArgument>
          <Expression>
            <CSharpDynamicGetMember Type=""System.Object"" Name=""foo"" Context=""Expressions"">
              <Object>
                <Parameter Type=""System.Object"" Id=""0"" Name=""d"" />
              </Object>
            </CSharpDynamicGetMember>
          </Expression>
        </DynamicCSharpArgument>
      </Right>
    </CSharpDynamicBinaryAssign>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_BBC0_6EA4();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_BBC0_6EA4() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_D860_6F20()
        {
            // (Expression<Func<dynamic, dynamic, dynamic>>)((l, r) => l ??= r.foo)
            var actual = GetDebugView(@"(Expression<Func<dynamic, dynamic, dynamic>>)((l, r) => l ??= r.foo)");
            var expected = @"
<Lambda Type=""System.Func`3[System.Object,System.Object,System.Object]"">
  <Parameters>
    <Parameter Type=""System.Object"" Id=""0"" Name=""l"" />
    <Parameter Type=""System.Object"" Id=""1"" Name=""r"" />
  </Parameters>
  <Body>
    <CSharpDynamicBinaryAssign Type=""System.Object"" OperationNodeType=""NullCoalescingAssign"">
      <Left>
        <DynamicCSharpArgument>
          <Expression>
            <Parameter Type=""System.Object"" Id=""0"" Name=""l"" />
          </Expression>
        </DynamicCSharpArgument>
      </Left>
      <Right>
        <DynamicCSharpArgument>
          <Expression>
            <CSharpDynamicGetMember Type=""System.Object"" Name=""foo"" Context=""Expressions"">
              <Object>
                <Parameter Type=""System.Object"" Id=""1"" Name=""r"" />
              </Object>
            </CSharpDynamicGetMember>
          </Expression>
        </DynamicCSharpArgument>
      </Right>
    </CSharpDynamicBinaryAssign>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_D860_6F20();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_D860_6F20() => INCONCLUSIVE(); }

        [TestMethod]
        public void CompilerTest_7A6C_546D()
        {
            // (Expression<Func<dynamic, dynamic, dynamic>>)((l, r) => l ??= r[int.Parse("0")])
            var actual = GetDebugView(@"(Expression<Func<dynamic, dynamic, dynamic>>)((l, r) => l ??= r[int.Parse(""0"")])");
            var expected = @"
<Lambda Type=""System.Func`3[System.Object,System.Object,System.Object]"">
  <Parameters>
    <Parameter Type=""System.Object"" Id=""0"" Name=""l"" />
    <Parameter Type=""System.Object"" Id=""1"" Name=""r"" />
  </Parameters>
  <Body>
    <CSharpDynamicBinaryAssign Type=""System.Object"" OperationNodeType=""NullCoalescingAssign"">
      <Left>
        <DynamicCSharpArgument>
          <Expression>
            <Parameter Type=""System.Object"" Id=""0"" Name=""l"" />
          </Expression>
        </DynamicCSharpArgument>
      </Left>
      <Right>
        <DynamicCSharpArgument>
          <Expression>
            <CSharpDynamicGetIndex Type=""System.Object"" Context=""Expressions"">
              <Object>
                <Parameter Type=""System.Object"" Id=""1"" Name=""r"" />
              </Object>
              <Arguments>
                <DynamicCSharpArgument Flags=""UseCompileTimeType"">
                  <Expression>
                    <Call Type=""System.Int32"" Method=""Int32 Parse(System.String)"">
                      <Arguments>
                        <Constant Type=""System.String"" Value=""0"" />
                      </Arguments>
                    </Call>
                  </Expression>
                </DynamicCSharpArgument>
              </Arguments>
            </CSharpDynamicGetIndex>
          </Expression>
        </DynamicCSharpArgument>
      </Right>
    </CSharpDynamicBinaryAssign>
  </Body>
</Lambda>";
            Assert.AreEqual(expected.TrimStart('\r', '\n'), actual);
            Verify.CompilerTest_7A6C_546D();
        }

        partial class Review { /* override in .Verify.cs */ public virtual void CompilerTest_7A6C_546D() => INCONCLUSIVE(); }

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
    partial class CompilerTests_CSharp80_NullCoalescingAssignment
    {
        partial class Reviewed
        {
            public override void CompilerTest_A465_3AAF() => OK();
            public override void CompilerTest_07E0_24A3() => OK();
            public override void CompilerTest_22AE_900C() => OK();
            public override void CompilerTest_0FE1_86B6() => OK();
            public override void CompilerTest_75E3_213A() => OK();
            public override void CompilerTest_9CD5_03D0() => OK();
            public override void CompilerTest_330E_2E05() => OK();
            public override void CompilerTest_B81A_515D() => OK();
            public override void CompilerTest_7AF5_0A0E() => OK();
            public override void CompilerTest_50DE_079F() => OK();
            public override void CompilerTest_FE8E_B94B() => OK();
            public override void CompilerTest_9440_85D5() => OK();
            public override void CompilerTest_2C1D_9A84() => OK();
            public override void CompilerTest_F57B_8C67() => OK();
            public override void CompilerTest_734B_65DC() => OK();
            public override void CompilerTest_BBC0_6EA4() => OK();
            public override void CompilerTest_D860_6F20() => OK();
            public override void CompilerTest_7A6C_546D() => OK();
        }
    }
}
*/
}
