using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using KRPC.Service.Messages;
using NUnit.Framework;

namespace KRPC.Test.Service
{
    [TestFixture]
    [SuppressMessage ("Gendarme.Rules.Naming", "AvoidRedundancyInTypeNameRule")]
    [SuppressMessage ("Gendarme.Rules.Portability", "NewLineLiteralRule")]
    public class ScannerTest
    {
        Services services;

        [SetUp]
        public void SetUp ()
        {
            services = KRPC.Service.KRPC.GetServices ();
            Assert.IsNotNull (services);
        }

        [Test]
        public void Services ()
        {
            Assert.AreEqual (4, services.ServicesList.Count);
            CollectionAssert.AreEquivalent (
                new [] { "KRPC", "TestService", "TestService2", "TestService3Name" },
                services.ServicesList.Select (x => x.Name).ToList ());
        }

        [Test]
        public void TestService ()
        {
            var service = services.ServicesList.First (x => x.Name == "TestService");
            Assert.AreEqual (37, service.Procedures.Count);
            Assert.AreEqual (2, service.Classes.Count);
            Assert.AreEqual (1, service.Enumerations.Count);
            Assert.AreEqual ("<doc>\n  <summary>\nTest service documentation.\n</summary>\n</doc>", service.Documentation);
        }

        [Test]
        public void TestService2 ()
        {
            var service = services.ServicesList.First (x => x.Name == "TestService2");
            Assert.AreEqual (2, service.Procedures.Count);
            Assert.AreEqual (0, service.Classes.Count);
            Assert.AreEqual (0, service.Enumerations.Count);
            Assert.AreEqual ("<doc>\n  <summary>\nTestService2 documentation.\n</summary>\n</doc>", service.Documentation);
        }

        [Test]
        public void TestService3Name ()
        {
            var service = services.ServicesList.First (x => x.Name == "TestService3Name");
            Assert.AreEqual (1, service.Procedures.Count);
            Assert.AreEqual (1, service.Classes.Count);
            Assert.AreEqual (0, service.Enumerations.Count);
            Assert.AreEqual (String.Empty, service.Documentation);
        }

        [Test]
        [SuppressMessage ("Gendarme.Rules.Maintainability", "AvoidComplexMethodsRule")]
        [SuppressMessage ("Gendarme.Rules.Smells", "AvoidLongMethodsRule")]
        public void TestServiceProcedures ()
        {
            var service = services.ServicesList.First (x => x.Name == "TestService");
            int foundProcedures = 0;
            foreach (var proc in service.Procedures) {
                if (proc.Name == "ProcedureNoArgsNoReturn") {
                    MessageAssert.HasNoParameters (proc);
                    MessageAssert.HasNoReturnType (proc);
                    MessageAssert.HasDocumentation (proc, "<doc>\n  <summary>\nProcedure with no return arguments.\n</summary>\n</doc>");
                } else if (proc.Name == "ProcedureSingleArgNoReturn") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(Response), "data");
                    MessageAssert.HasNoReturnType (proc);
                    MessageAssert.HasDocumentation (proc, "<doc>\n  <summary>\nProcedure with a single return argument.\n</summary>\n</doc>");
                } else if (proc.Name == "ProcedureThreeArgsNoReturn") {
                    MessageAssert.HasParameters (proc, 3);
                    MessageAssert.HasParameter (proc, 0, typeof(Response), "x");
                    MessageAssert.HasParameter (proc, 1, typeof(Request), "y");
                    MessageAssert.HasParameter (proc, 2, typeof(Response), "z");
                    MessageAssert.HasNoReturnType (proc);
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "ProcedureNoArgsReturns") {
                    MessageAssert.HasNoParameters (proc);
                    MessageAssert.HasReturnType (proc, typeof(Response));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "ProcedureSingleArgReturns") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(Response), "data");
                    MessageAssert.HasReturnType (proc, typeof(Response));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "ProcedureWithValueTypes") {
                    MessageAssert.HasParameters (proc, 3);
                    MessageAssert.HasParameter (proc, 0, typeof(float), "x");
                    MessageAssert.HasParameter (proc, 1, typeof(string), "y");
                    MessageAssert.HasParameter (proc, 2, typeof(byte[]), "z");
                    MessageAssert.HasReturnType (proc, typeof(int));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "get_PropertyWithGetAndSet") {
                    MessageAssert.HasNoParameters (proc);
                    MessageAssert.HasReturnType (proc, typeof(string));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "set_PropertyWithGetAndSet") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(string), "value");
                    MessageAssert.HasNoReturnType (proc);
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "get_PropertyWithGet") {
                    MessageAssert.HasNoParameters (proc);
                    MessageAssert.HasReturnType (proc, typeof(string));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "set_PropertyWithSet") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(string), "value");
                    MessageAssert.HasNoReturnType (proc);
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "CreateTestObject") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(string), "value");
                    MessageAssert.HasReturnType (proc, typeof(TestService.TestClass));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "DeleteTestObject") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(TestService.TestClass), "obj");
                    MessageAssert.HasNoReturnType (proc);
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "EchoTestObject") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(TestService.TestClass), "obj");
                    MessageAssert.HasReturnType (proc, typeof(TestService.TestClass));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "TestClass_FloatToString") {
                    MessageAssert.HasParameters (proc, 2);
                    MessageAssert.HasParameter (proc, 0, typeof(TestService.TestClass), "this");
                    MessageAssert.HasParameter (proc, 1, typeof(float), "x");
                    MessageAssert.HasReturnType (proc, typeof(string));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "TestClass_ObjectToString") {
                    MessageAssert.HasParameters (proc, 2);
                    MessageAssert.HasParameter (proc, 0, typeof(TestService.TestClass), "this");
                    MessageAssert.HasParameter (proc, 1, typeof(TestService.TestClass), "other");
                    MessageAssert.HasReturnType (proc, typeof(string));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "TestClass_IntToString") {
                    MessageAssert.HasParameters (proc, 2);
                    MessageAssert.HasParameter (proc, 0, typeof(TestService.TestClass), "this");
                    MessageAssert.HasParameterWithDefaultValue (proc, 1, typeof(int), "x", 42);
                    MessageAssert.HasReturnType (proc, typeof(string));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "TestClass_get_IntProperty") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(TestService.TestClass), "this");
                    MessageAssert.HasReturnType (proc, typeof(int));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "TestClass_set_IntProperty") {
                    MessageAssert.HasParameters (proc, 2);
                    MessageAssert.HasParameter (proc, 0, typeof(TestService.TestClass), "this");
                    MessageAssert.HasParameter (proc, 1, typeof(int), "value");
                    MessageAssert.HasNoReturnType (proc);
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "TestClass_get_ObjectProperty") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(TestService.TestClass), "this");
                    MessageAssert.HasReturnType (proc, typeof(TestService.TestClass));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "TestClass_set_ObjectProperty") {
                    MessageAssert.HasParameters (proc, 2);
                    MessageAssert.HasParameter (proc, 0, typeof(TestService.TestClass), "this");
                    MessageAssert.HasParameter (proc, 1, typeof(TestService.TestClass), "value");
                    MessageAssert.HasNoReturnType (proc);
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "TestClass_static_StaticMethod") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameterWithDefaultValue (proc, 0, typeof(string), "a", String.Empty);
                    MessageAssert.HasReturnType (proc, typeof(string));
                } else if (proc.Name == "TestTopLevelClass_AMethod") {
                    MessageAssert.HasParameters (proc, 2);
                    MessageAssert.HasParameter (proc, 0, typeof(TestTopLevelClass), "this");
                    MessageAssert.HasParameter (proc, 1, typeof(int), "x");
                    MessageAssert.HasReturnType (proc, typeof(string));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "TestTopLevelClass_get_AProperty") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(TestTopLevelClass), "this");
                    MessageAssert.HasReturnType (proc, typeof(string));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "TestTopLevelClass_set_AProperty") {
                    MessageAssert.HasParameters (proc, 2);
                    MessageAssert.HasParameter (proc, 0, typeof(TestTopLevelClass), "this");
                    MessageAssert.HasParameter (proc, 1, typeof(string), "value");
                    MessageAssert.HasNoReturnType (proc);
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "ProcedureSingleOptionalArgNoReturn") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameterWithDefaultValue (proc, 0, typeof(string), "x", "foo");
                    MessageAssert.HasNoReturnType (proc);
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "ProcedureThreeOptionalArgsNoReturn") {
                    MessageAssert.HasParameters (proc, 3);
                    MessageAssert.HasParameter (proc, 0, typeof(float), "x");
                    MessageAssert.HasParameterWithDefaultValue (proc, 1, typeof(string), "y", "jeb");
                    MessageAssert.HasParameterWithDefaultValue (proc, 2, typeof(int), "z", 42);
                    MessageAssert.HasNoReturnType (proc);
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "ProcedureOptionalNullArg") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameterWithDefaultValue (proc, 0, typeof(TestService.TestClass), "x", null);
                    MessageAssert.HasNoReturnType (proc);
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "ProcedureEnumArg") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(TestService.TestEnum), "x");
                    MessageAssert.HasNoReturnType (proc);
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "ProcedureEnumReturn") {
                    MessageAssert.HasNoParameters (proc);
                    MessageAssert.HasReturnType (proc, typeof(TestService.TestEnum));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "BlockingProcedureNoReturn") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(int), "n");
                    MessageAssert.HasNoReturnType (proc);
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "BlockingProcedureReturns") {
                    MessageAssert.HasParameters (proc, 2);
                    MessageAssert.HasParameter (proc, 0, typeof(int), "n");
                    MessageAssert.HasParameterWithDefaultValue (proc, 1, typeof(int), "sum", 0);
                    MessageAssert.HasReturnType (proc, typeof(int));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "EchoList") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(IList<string>), "l");
                    MessageAssert.HasReturnType (proc, typeof(IList<string>));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "EchoDictionary") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(IDictionary<int,string>), "d");
                    MessageAssert.HasReturnType (proc, typeof(IDictionary<int,string>));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "EchoSet") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(HashSet<int>), "h");
                    MessageAssert.HasReturnType (proc, typeof(HashSet<int>));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "EchoTuple") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(KRPC.Utils.Tuple<int,bool>), "t");
                    MessageAssert.HasReturnType (proc, typeof(KRPC.Utils.Tuple<int,bool>));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "EchoNestedCollection") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(IDictionary<int,IList<string>>), "c");
                    MessageAssert.HasReturnType (proc, typeof(IDictionary<int,IList<string>>));
                    MessageAssert.HasNoDocumentation (proc);
                } else if (proc.Name == "EchoListOfObjects") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(IList<TestService.TestClass>), "l");
                    MessageAssert.HasReturnType (proc, typeof(IList<TestService.TestClass>));
                    MessageAssert.HasNoDocumentation (proc);
                } else {
                    Assert.Fail ("Procedure not found");
                }
                foundProcedures++;
            }
            Assert.AreEqual (37, foundProcedures);
            Assert.AreEqual (37, service.Procedures.Count);
        }

        [Test]
        public void TestServiceClasses ()
        {
            var service = services.ServicesList.First (x => x.Name == "TestService");
            int foundClasses = 0;
            foreach (var cls in service.Classes) {
                if (cls.Name == "TestClass") {
                    MessageAssert.HasNoDocumentation (cls);
                } else if (cls.Name == "TestTopLevelClass") {
                    MessageAssert.HasDocumentation (cls, "<doc>\n  <summary>\nA class defined at the top level, but included in a service\n</summary>\n</doc>");
                } else {
                    Assert.Fail ();
                }
                foundClasses++;
            }
            Assert.AreEqual (2, foundClasses);
            Assert.AreEqual (2, service.Classes.Count);
        }

        [Test]
        public void TestServiceEnumerations ()
        {
            var service = services.ServicesList.First (x => x.Name == "TestService");
            int foundEnumerations = 0;
            foreach (var enumeration in service.Enumerations) {
                if (enumeration.Name == "TestEnum") {
                    MessageAssert.HasDocumentation (enumeration, "<doc>\n  <summary>\nDocumentation string for TestEnum.\n</summary>\n</doc>");
                    MessageAssert.HasValues (enumeration, 3);
                    MessageAssert.HasValue (enumeration, 0, "X", 0, "<doc>\n  <summary>\nDocumented enum field\n</summary>\n</doc>");
                    MessageAssert.HasValue (enumeration, 1, "Y", 1);
                    MessageAssert.HasValue (enumeration, 2, "Z", 2);
                } else {
                    Assert.Fail ();
                }
                foundEnumerations++;
            }
            Assert.AreEqual (1, foundEnumerations);
            Assert.AreEqual (1, service.Enumerations.Count);
        }

        [Test]
        public void TestService2Procedures ()
        {
            var service = services.ServicesList.First (x => x.Name == "TestService2");
            int foundProcedures = 0;
            foreach (var proc in service.Procedures) {
                if (proc.Name == "ClassTypeFromOtherServiceAsParameter") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(TestService.TestClass), "obj");
                    MessageAssert.HasReturnType (proc, typeof(int));
                    MessageAssert.HasDocumentation (proc, "<doc>\n  <summary>\nTestService2 procedure documentation.\n</summary>\n</doc>");
                } else if (proc.Name == "ClassTypeFromOtherServiceAsReturn") {
                    MessageAssert.HasParameters (proc, 1);
                    MessageAssert.HasParameter (proc, 0, typeof(string), "value");
                    MessageAssert.HasReturnType (proc, typeof(TestService.TestClass));
                    MessageAssert.HasNoDocumentation (proc);
                } else {
                    Assert.Fail ();
                }
                foundProcedures++;
            }
            Assert.AreEqual (2, foundProcedures);
            Assert.AreEqual (2, service.Procedures.Count);
        }
    }
}
