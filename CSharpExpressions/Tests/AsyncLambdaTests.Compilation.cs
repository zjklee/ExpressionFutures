﻿// Prototyping extended expression trees for C#.
//
// bartde - October 2015

using Microsoft.CSharp.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static Tests.ReflectionUtils;

namespace Tests
{
    partial class AsyncLambdaTests
    {
        [TestMethod]
        public void AsyncLambda_Compilation_NotInFilter()
        {
            var p = Expression.Parameter(typeof(Exception));

            var expr = Expression.TryCatch(
                Expression.Empty(),
                Expression.Catch(
                    p,
                    Expression.Empty(),
                    CSharpExpression.Await(Expression.Constant(Task.FromResult(true)))
                )
            );

            var e = CSharpExpression.AsyncLambda<Func<Task>>(expr);
            AssertEx.Throws<InvalidOperationException>(() => e.Compile());
        }

        [TestMethod]
        public void AsyncLambda_Compilation_NotInLock()
        {
            var p = Expression.Parameter(typeof(Exception));

            var expr = CSharpExpression.Lock(
                Expression.Default(typeof(object)),
                CSharpExpression.Await(Expression.Constant(Task.FromResult(42)))
            );

            var e = CSharpExpression.AsyncLambda<Func<Task>>(expr);
            AssertEx.Throws<InvalidOperationException>(() => e.Compile());
        }

        [TestMethod]
        public void AsyncLambda_Compilation_NotInLambda()
        {
            var p = Expression.Parameter(typeof(Exception));

            var expr = Expression.Lambda(
                CSharpExpression.Await(Expression.Constant(Task.FromResult(42)))
            );

            var e = CSharpExpression.AsyncLambda<Func<Task<Func<int>>>>(expr);
            AssertEx.Throws<InvalidOperationException>(() => e.Compile());
        }

        [TestMethod]
        public void AsyncLambda_Compilation_NestedLambda()
        {
            var p = Expression.Parameter(typeof(Exception));

            var expr = Expression.Invoke(Expression.Lambda(Expression.Constant(42)));

            var e = CSharpExpression.AsyncLambda<Func<Task<int>>>(expr);
            Assert.AreEqual(42, e.Compile()().Result);
        }

        [TestMethod]
        public void AsyncLambda_Compilation_NestedAsyncLambda()
        {
            var p = Expression.Parameter(typeof(Exception));

            var expr = CSharpExpression.AsyncLambda(
                CSharpExpression.Await(Expression.Constant(Task.FromResult(42)))
            );

            var e = CSharpExpression.AsyncLambda<Func<Task<Func<Task<int>>>>>(expr);
            Assert.AreEqual(42, e.Compile()().Result().Result);
        }

        [TestMethod]
        [Ignore] // DynamicMethod does not support BeginExceptFilterBlock (see https://github.com/dotnet/coreclr/issues/1764)
        public void AsyncLambda_Compilation_NotInFilter_NoFalsePositive()
        {
            var p = Expression.Parameter(typeof(Exception));

            var expr = Expression.TryCatch(
                Expression.Empty(),
                Expression.Catch(
                    p,
                    CSharpExpression.Await(Expression.Constant(Task.FromResult(true), typeof(Task))),
                    Expression.Constant(true)
                )
            );

            var e = CSharpExpression.AsyncLambda<Func<Task>>(expr);
            AssertEx.Throws<InvalidOperationException>(() => e.Compile());
        }

        [TestMethod]
        public void AsyncLambda_Compilation_NotInLock_NoFalsePositive()
        {
            var p = Expression.Parameter(typeof(Exception));

            var expr = CSharpExpression.Lock(
                CSharpExpression.Await(Expression.Constant(Task.FromResult(default(object)))),
                Expression.Empty()
            );

            var e = CSharpExpression.AsyncLambda<Func<Task>>(expr);
            AssertEx.Throws<InvalidOperationException>(() => e.Compile());
        }

        [TestMethod]
        public void AsyncLambda_Compilation_Simple0()
        {
            var p = Expression.Parameter(typeof(int));

            var e1 = CSharpExpression.AsyncLambda<Func<int, Task<int>>>(p, p);
            var e2 = CSharpExpression.AsyncLambda(typeof(Func<int, Task<int>>), p, p);
            var e3 = CSharpExpression.AsyncLambda<Func<int, Task<int>>>(p, p);

            foreach (AsyncCSharpExpression<Func<int, Task<int>>> e in new[] { e1, e2, e3 })
            {
                Assert.AreEqual(42, e.Compile()(42).Result);
            }
        }

        [TestMethod]
        public void AsyncLambda_Compilation_Simple1()
        {
            var e = CSharpExpression.AsyncLambda<Func<Task<int>>>(Expression.Constant(42));
            var f = e.Compile();
            var t = f();
            var r = t.Result;
            Assert.AreEqual(42, r);
        }

        [TestMethod]
        public void AsyncLambda_Compilation_Simple2()
        {
            var e = CSharpExpression.AsyncLambda<Func<Task>>(Expression.Constant(42));
            var f = e.Compile();
            var t = f();
            t.Wait();
        }

        [TestMethod]
        public void AsyncLambda_Compilation_Simple3()
        {
            var log = new List<string>();
            var a = (Expression<Action>)(() => log.Add("OK"));

            var e = CSharpExpression.AsyncLambda<Func<Task>>(a.Body);
            var f = e.Compile();
            var t = f();

            // Add happens on sync code path
            Assert.IsTrue(new[] { "OK" }.SequenceEqual(log));

            t.Wait();
        }

        [TestMethod]
        public void AsyncLambda_Compilation_Simple4()
        {
            var log = new List<string>();
            var a = (Expression<Action>)(() => log.Add("OK"));

            var e = CSharpExpression.AsyncLambda<Action>(a.Body);
            var f = e.Compile();
            f();

            // Add happens on sync code path
            Assert.IsTrue(new[] { "OK" }.SequenceEqual(log));
        }

        [TestMethod]
        public void AsyncLambda_Compilation_Simple5()
        {
            var v = Expression.Constant(Task.FromResult(42));
            var e = CSharpExpression.AsyncLambda<Func<Task<int>>>(CSharpExpression.Await(v));
            var f = e.Compile();
            var t = f();
            var r = t.Result;
            Assert.AreEqual(42, r);
        }

        [TestMethod]
        public void AsyncLambda_Compilation_Simple6()
        {
            var l = Expression.Label(typeof(int));
            var e = CSharpExpression.AsyncLambda<Func<Task<int>>>(Expression.Block(Expression.Return(l, Expression.Constant(42)), Expression.Label(l, Expression.Constant(0))));
            var f = e.Compile();
            var t = f();
            var r = t.Result;
            Assert.AreEqual(42, r);
        }

        [TestMethod]
        public void AsyncLambda_Compilation_CustomGetAwaiter()
        {
            var m = MethodInfoOf(() => GetAwaiter<int>(default(Task<int>)));
            var v = Expression.Constant(Task.FromResult(42));
            var e = CSharpExpression.AsyncLambda<Func<Task<int>>>(CSharpExpression.Await(v, m));
            var f = e.Compile();
            var t = f();
            var r = t.Result;
            Assert.AreEqual(42, r);
        }

        private static TaskAwaiter<T> GetAwaiter<T>(Task<T> task)
        {
            return task.GetAwaiter();
        }

        [TestMethod]
        public void AsyncLambda_Compilation_Spilling()
        {
            var v1 = Expression.Constant(Task.FromResult(1));
            var v2 = Expression.Constant(Task.FromResult(2));
            var add = Expression.Add(CSharpExpression.Await(v1), CSharpExpression.Await(v2));
            var e = CSharpExpression.AsyncLambda<Func<Task<int>>>(add);
            var f = e.Compile();
            var t = f();
            var r = t.Result;
            Assert.AreEqual(3, r);
        }

        [TestMethod]
        public void AsyncLambda_Compilation_Hoisting()
        {
            var fromResultMethod = MethodInfoOf(() => Task.FromResult(default(int)));
            var i = Expression.Parameter(typeof(int));
            var res = Expression.Parameter(typeof(int));
            var brk = Expression.Label();
            var e = CSharpExpression.AsyncLambda<Func<Task<int>>>(
                Expression.Block(
                    new[] { i, res },
                    Expression.Assign(i, Expression.Constant(0)),
                    Expression.Loop(
                        Expression.Block(
                            Expression.IfThen(
                                Expression.Equal(i, Expression.Constant(10)),
                                Expression.Break(brk)
                            ),
                            Expression.AddAssign(
                                res,
                                CSharpExpression.Await(Expression.Call(fromResultMethod, i))
                            ),
                            Expression.PostIncrementAssign(i)
                        ), brk
                    ),
                    res
                )
            );
            var f = e.Compile();
            var t = f();
            var r = t.Result;
            Assert.AreEqual(Enumerable.Range(0, 10).Sum(), r);
        }

        [TestMethod]
        public void AsyncLambda_Compilation_HoistingParameters()
        {
            var yield = ((Expression<Func<YieldAwaitable>>)(() => Task.Yield())).Body;
            var x = Expression.Parameter(typeof(int));
            var e = CSharpExpression.AsyncLambda<Func<int, Task<int>>>(
                Expression.Block(
                    CSharpExpression.Await(yield),
                    x
                ),
                x
            );
            var f = e.Compile();
            var t = f(42);
            var r = t.Result;
            Assert.AreEqual(42, r);
        }

        [TestMethod]
        public void AsyncLambda_Compilation_ResumeInTry1()
        {
            var yield = ((Expression<Func<YieldAwaitable>>)(() => Task.Yield())).Body;
            var e = CSharpExpression.AsyncLambda<Func<Task<int>>>(
                Expression.Block(
                    Expression.TryCatch(
                        Expression.Block(
                            CSharpExpression.Await(yield),
                            Expression.Constant(42)
                        ),
                        Expression.Catch(Expression.Parameter(typeof(Exception)),
                            Expression.Constant(-1)
                        )
                    )
                )
            );
            var f = e.Compile();
            var t = f();
            var r = t.Result;
            Assert.AreEqual(42, r);
        }

        [TestMethod]
        public void AsyncLambda_Compilation_ResumeInTry2()
        {
            var yield = ((Expression<Func<YieldAwaitable>>)(() => Task.Yield())).Body;
            var e = CSharpExpression.AsyncLambda<Func<Task<int>>>(
                Expression.Block(
                    Expression.TryCatch(
                        Expression.Block(
                            CSharpExpression.Await(yield),
                            Expression.Divide(Expression.Constant(1), Expression.Constant(0))
                        ),
                        Expression.Catch(Expression.Parameter(typeof(DivideByZeroException)),
                            Expression.Constant(-1)
                        )
                    )
                )
            );
            var f = e.Compile();
            var t = f();
            var r = t.Result;
            Assert.AreEqual(-1, r);
        }

        [TestMethod]
        public void AsyncLambda_Compilation_ResumeInTry3()
        {
            var p = Expression.Parameter(typeof(D));
            var yield = ((Expression<Func<YieldAwaitable>>)(() => Task.Yield())).Body;
            var e = CSharpExpression.AsyncLambda<Func<D, Task<int>>>(
                Expression.Block(
                    Expression.TryFinally(
                        Expression.Block(
                            CSharpExpression.Await(yield),
                            Expression.Call(p, typeof(D).GetMethod("Do"))
                        ),
                        Expression.Call(p, typeof(D).GetMethod("Dispose"))
                    )
                ),
                p
            );
            var d = new D();
            var f = e.Compile();
            var t = f(d);
            var r = t.Result;
            Assert.AreEqual(42, r);
            Assert.IsTrue(d.IsDisposed);
        }

        [TestMethod]
        public void AsyncLambda_Compilation_AwaitInFinally1()
        {
            var p = Expression.Parameter(typeof(D));
            var yield = ((Expression<Func<YieldAwaitable>>)(() => Task.Yield())).Body;
            var e = CSharpExpression.AsyncLambda<Func<D, Task<int>>>(
                Expression.TryFinally(
                    Expression.Call(p, typeof(D).GetMethod("Do")),
                    Expression.Block(
                        CSharpExpression.Await(yield),
                        Expression.Call(p, typeof(D).GetMethod("Dispose"))
                    )
                ),
                p
            );
            var d = new D();
            var f = e.Compile();
            var t = f(d);
            var r = t.Result;
            Assert.AreEqual(42, r);
            Assert.IsTrue(d.IsDisposed);
        }

        [TestMethod]
        public void AsyncLambda_Compilation_AwaitInFinally2()
        {
            var yield = ((Expression<Func<YieldAwaitable>>)(() => Task.Yield())).Body;
            var e = CSharpExpression.AsyncLambda<Func<Task<int>>>(
                Expression.TryCatch(
                    Expression.TryFinally(
                        Expression.Divide(Expression.Constant(1), Expression.Constant(0)),
                        CSharpExpression.Await(yield)
                    ),
                    Expression.Catch(Expression.Parameter(typeof(DivideByZeroException)),
                        Expression.Constant(-1)
                    )
                )
            );
            var f = e.Compile();
            var t = f();
            var r = t.Result;
            Assert.AreEqual(-1, r);
        }

        [TestMethod]
        public void AsyncLambda_Compilation_AwaitInFinally3()
        {
            var log = new List<string>();
            var logExpr = Expression.Constant(log);
            var add = MethodInfoOf((List<string> ss) => ss.Add(default(string)));
            var yield = ((Expression<Func<YieldAwaitable>>)(() => Task.Yield())).Body;
            var e = CSharpExpression.AsyncLambda<Func<Task>>(
                Expression.TryFinally(
                    Expression.Call(logExpr, add, Expression.Constant("T")),
                    Expression.Block(
                        Expression.Call(logExpr, add, Expression.Constant("FB")),
                        CSharpExpression.Await(yield),
                        Expression.Call(logExpr, add, Expression.Constant("FE"))
                    )
                )
            );
            var f = e.Compile();
            var t = f();
            t.Wait();
            Assert.IsTrue(new[] { "T", "FB", "FE" }.SequenceEqual(log));
        }

        [TestMethod]
        public void AsyncLambda_Compilation_AwaitInFinally4()
        {
            var log = new List<string>();
            var logExpr = Expression.Constant(log);
            var add = MethodInfoOf((List<string> ss) => ss.Add(default(string)));
            var yield = ((Expression<Func<YieldAwaitable>>)(() => Task.Yield())).Body;
            var e = CSharpExpression.AsyncLambda<Func<Task>>(
                Expression.TryCatch(
                    Expression.TryFinally(
                        Expression.Block(
                            Expression.Call(logExpr, add, Expression.Constant("T")),
                            Expression.Throw(Expression.Constant(new Exception()))
                        ),
                        Expression.Block(
                            Expression.Call(logExpr, add, Expression.Constant("FB")),
                            CSharpExpression.Await(yield),
                            Expression.Call(logExpr, add, Expression.Constant("FE"))
                        )
                    ),
                    Expression.Catch(Expression.Parameter(typeof(Exception)),
                        Expression.Call(logExpr, add, Expression.Constant("C"))
                    )
                )
            );
            var f = e.Compile();
            var t = f();
            t.Wait();
            Assert.IsTrue(new[] { "T", "FB", "FE", "C" }.SequenceEqual(log));
        }

        [TestMethod]
        public void AsyncLambda_Compilation_AwaitInFault1()
        {
            var p = Expression.Parameter(typeof(D));
            var yield = ((Expression<Func<YieldAwaitable>>)(() => Task.Yield())).Body;
            var e = CSharpExpression.AsyncLambda<Func<D, Task<int>>>(
                Expression.TryFault(
                    Expression.Call(p, typeof(D).GetMethod("Do")),
                    Expression.Block(
                        CSharpExpression.Await(yield),
                        Expression.Call(p, typeof(D).GetMethod("Dispose"))
                    )
                ),
                p
            );
            var d = new D();
            var f = e.Compile();
            var t = f(d);
            var r = t.Result;
            Assert.AreEqual(42, r);
            Assert.IsFalse(d.IsDisposed);
        }

        [TestMethod]
        public void AsyncLambda_Compilation_AwaitInFault2()
        {
            var yield = ((Expression<Func<YieldAwaitable>>)(() => Task.Yield())).Body;
            var e = CSharpExpression.AsyncLambda<Func<Task<int>>>(
                Expression.TryCatch(
                    Expression.TryFault(
                        Expression.Divide(Expression.Constant(1), Expression.Constant(0)),
                        CSharpExpression.Await(yield)
                    ),
                    Expression.Catch(Expression.Parameter(typeof(DivideByZeroException)),
                        Expression.Constant(-1)
                    )
                )
            );
            var f = e.Compile();
            var t = f();
            var r = t.Result;
            Assert.AreEqual(-1, r);
        }

        [TestMethod]
        public void AsyncLambda_Compilation_AwaitInFault3()
        {
            var log = new List<string>();
            var logExpr = Expression.Constant(log);
            var add = MethodInfoOf((List<string> ss) => ss.Add(default(string)));
            var yield = ((Expression<Func<YieldAwaitable>>)(() => Task.Yield())).Body;
            var e = CSharpExpression.AsyncLambda<Func<Task>>(
                Expression.TryFault(
                    Expression.Call(logExpr, add, Expression.Constant("T")),
                    Expression.Block(
                        Expression.Call(logExpr, add, Expression.Constant("FB")),
                        CSharpExpression.Await(yield),
                        Expression.Call(logExpr, add, Expression.Constant("FE"))
                    )
                )
            );
            var f = e.Compile();
            var t = f();
            t.Wait();
            Assert.IsTrue(new[] { "T" }.SequenceEqual(log));
        }

        [TestMethod]
        public void AsyncLambda_Compilation_AwaitInFault4()
        {
            var log = new List<string>();
            var logExpr = Expression.Constant(log);
            var add = MethodInfoOf((List<string> ss) => ss.Add(default(string)));
            var yield = ((Expression<Func<YieldAwaitable>>)(() => Task.Yield())).Body;
            var e = CSharpExpression.AsyncLambda<Func<Task>>(
                Expression.TryCatch(
                    Expression.TryFault(
                        Expression.Block(
                            Expression.Call(logExpr, add, Expression.Constant("T")),
                            Expression.Throw(Expression.Constant(new Exception()))
                        ),
                        Expression.Block(
                            Expression.Call(logExpr, add, Expression.Constant("FB")),
                            CSharpExpression.Await(yield),
                            Expression.Call(logExpr, add, Expression.Constant("FE"))
                        )
                    ),
                    Expression.Catch(Expression.Parameter(typeof(Exception)),
                        Expression.Call(logExpr, add, Expression.Constant("C"))
                    )
                )
            );
            var f = e.Compile();
            var t = f();
            t.Wait();
            Assert.IsTrue(new[] { "T", "FB", "FE", "C" }.SequenceEqual(log));
        }

        [TestMethod]
        public void AsyncLambda_Compilation_NestedFinally()
        {
            var log = new List<string>();
            var logExpr = Expression.Constant(log);
            var add = MethodInfoOf((List<string> ss) => ss.Add(default(string)));
            var yield = ((Expression<Func<YieldAwaitable>>)(() => Task.Yield())).Body;
            var e = CSharpExpression.AsyncLambda<Func<Task>>(
                Expression.TryFinally(
                    Expression.Block(
                        Expression.Call(logExpr, add, Expression.Constant("T1"))
                    ),
                    Expression.Block(
                        Expression.Call(logExpr, add, Expression.Constant("FB1")),
                         Expression.TryFinally(
                            Expression.Block(
                                Expression.Call(logExpr, add, Expression.Constant("T2"))
                            ),
                            Expression.Block(
                                Expression.Call(logExpr, add, Expression.Constant("FB2")),
                                CSharpExpression.Await(yield),
                                Expression.Call(logExpr, add, Expression.Constant("FE2"))
                            )
                        ),
                        Expression.Call(logExpr, add, Expression.Constant("FE1"))
                    )
                )
            );
            var f = e.Compile();
            var t = f();
            t.Wait();
            Assert.IsTrue(new[] { "T1", "FB1", "T2", "FB2", "FE2", "FE1" }.SequenceEqual(log));
        }

        [TestMethod]
        public void AsyncLambda_Compilation_UnpendBranch1()
        {
            var log = new List<string>();
            var logExpr = Expression.Constant(log);
            var add = MethodInfoOf((List<string> ss) => ss.Add(default(string)));
            var yield = ((Expression<Func<YieldAwaitable>>)(() => Task.Yield())).Body;
            var lbl = Expression.Label();
            var brk = Expression.Label();
            var cnt = Expression.Label();
            var lbm = Expression.Label();
            var e = CSharpExpression.AsyncLambda<Func<Task>>(
                Expression.Block(
                    Expression.TryFinally(
                        Expression.Block(
                            Expression.Call(logExpr, add, Expression.Constant("T")),
                            Expression.Goto(lbm),
                            Expression.Loop(Expression.Empty(), brk, cnt),
                            Expression.Label(lbm),
                            Expression.Goto(lbl)
                        ),
                        Expression.Block(
                            Expression.Call(logExpr, add, Expression.Constant("FB")),
                            CSharpExpression.Await(yield),
                            Expression.Call(logExpr, add, Expression.Constant("FE"))
                        )
                    ),
                    Expression.Call(logExpr, add, Expression.Constant("X")),
                    Expression.Label(lbl),
                    Expression.Call(logExpr, add, Expression.Constant("O"))
                )
            );
            var f = e.Compile();
            var t = f();
            t.Wait();
            Assert.IsTrue(new[] { "T", "FB", "FE", "O" }.SequenceEqual(log));
        }

        [TestMethod]
        public void AsyncLambda_Compilation_UnpendBranch2()
        {
            var log = new List<string>();
            var logExpr = Expression.Constant(log);
            var add = MethodInfoOf((List<string> ss) => ss.Add(default(string)));
            var yield = ((Expression<Func<YieldAwaitable>>)(() => Task.Yield())).Body;
            var lbl = Expression.Label(typeof(int), "l");
            var brk = Expression.Label("b");
            var cnt = Expression.Label("c");
            var lbm = Expression.Label("m");
            var e = CSharpExpression.AsyncLambda<Func<Task<int>>>(
                Expression.Block(
                    Expression.TryFinally(
                        Expression.Block(
                            Expression.Call(logExpr, add, Expression.Constant("T")),
                            Expression.Goto(lbm),
                            Expression.Loop(Expression.Empty(), brk, cnt),
                            Expression.Label(lbm),
                            Expression.Goto(lbl, Expression.Constant(42))
                        ),
                        Expression.Block(
                            Expression.Call(logExpr, add, Expression.Constant("FB")),
                            CSharpExpression.Await(yield),
                            Expression.Call(logExpr, add, Expression.Constant("FE"))
                        )
                    ),
                    Expression.Call(logExpr, add, Expression.Constant("X")),
                    Expression.Label(lbl, Expression.Constant(0))
                )
            );
            var f = e.Compile();
            var t = f();
            var r = t.Result;
            Assert.AreEqual(42, r);
            Assert.IsTrue(new[] { "T", "FB", "FE" }.SequenceEqual(log));
        }

        [TestMethod]
        public void AsyncLambda_Compilation_ReducibleNodes()
        {
            var fromResultMethod = MethodInfoOf(() => Task.FromResult(default(int)));
            var i = Expression.Parameter(typeof(int));
            var res = Expression.Parameter(typeof(int));
            var brk = Expression.Label();
            var e = CSharpExpression.AsyncLambda<Func<Task<int>>>(
                Expression.Block(
                    new[] { i, res },
                    Expression.Assign(i, Expression.Constant(0)),
                    CSharpExpression.While(Expression.LessThan(i, Expression.Constant(10)),
                        Expression.Block(
                            Expression.AddAssign(
                                res,
                                CSharpExpression.Await(Expression.Call(fromResultMethod, i))
                            ),
                            Expression.PostIncrementAssign(i)
                        ), brk
                    ),
                    res
                )
            );
            var f = e.Compile();
            var t = f();
            var r = t.Result;
            Assert.AreEqual(Enumerable.Range(0, 10).Sum(), r);
        }

        [TestMethod]
        public void AsyncLambda_Compilation_NoStaticType()
        {
            var e = CSharpExpression.AsyncLambda(Expression.Constant(42));
            var f = (Func<Task<int>>)e.Compile();
            var t = f();
            var r = t.Result;
            Assert.AreEqual(42, r);
        }

        class D : IDisposable
        {
            public bool IsDisposed;

            public int Do()
            {
                if (IsDisposed)
                    throw new ObjectDisposedException("this");

                return 42;
            }

            public void Dispose()
            {
                IsDisposed = true;
            }
        }
    }
}
