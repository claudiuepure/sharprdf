﻿using Microsoft.CodeAnalysis.CSharp;
using Sharprdf.Core;
using Sharprdf.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF.Writing;

namespace Sharprdf.Console
{
    class Program
    {
        #region Constants

        private const string ScMinimum =
@"using System;

namespace HelloWorld
{
    public abstract class Car
    {
        public void StratEngine()
        {
            Console.WriteLine(""Start Engine"");
        }
    }
}";
        private const string Sc1 =
@"using System;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public abstract class TestAttribute : Attribute { }

public struct TestStruct { int x; }

public enum TestEnum { a, b, c }

public delegate void Test();

public interface InterTest { }

public static class A { }

public sealed class B { }


namespace HelloWorld
{
    using System.Collections.Concurrent;
    
    public delegate void DelegateNamespaceTest();

    namespace InnerNamespace
    {
        interface IInnerInterface {}
        
        class Test
        {
        }
    }
    
    public abstract class Shape
    {
        public delegate void DelegateClassTest();
        public abstract double CalculateSurface();
    }
        
    public class Rectangle : Shape
    {
        private double _width;
        private double _height;
        
        public Rectangle(double width, double height)
        {
            _width = width;
            _height = height;
        }
    
        public override double CalculateSurface()
        {
            return _width * _height;
        }
    }

    public class Square : Shape
    {
        private double size;
        
        public Square(double size)
        {
            _size = size;
        }
    
        public override double CalculateSurface()
        {
            return _size * _size;
        }
    }
}";

        private const string Sc2 =
@"
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
public abstract class Ta:Attribute { }
public struct Tx { int x; }

public enum Ex { a, b, c }
[Serializable]
public delegate void Test();
public interface InterTest { }
public static class A { }
[Serializable]
public sealed class B { }

namespace RoslynConsoleApp
{
    using System.Collections.Concurrent;
    public delegate void DelegateNamespaceTest();

    namespace TestNamespace
    {
        //=========== USINGS =============
        using System;

        //=========== NAMESPACES ==================
        namespace Test { }

        // ============ DECLARATIONS ================
        // simple declarations (no static, sealed, abstract, virtual)
        delegate void TestDeleg();
        enum EEnum { a, b, c }
        interface ITest { }
        [Serializable]
        struct SStruct { }

        //class declarations
        class CC { }
        static class CS { }
        abstract class CA { }
        sealed class CSE { }

        //============= MEMBERS ================
        //No members
        delegate void Del();
        enum ETestEnum { a, b, c, s}

        //only props and methods members
        public interface IParent
        {
            int MyProperty { get; set; }
            void Method();
        }
        public interface ITestInterface : IParent
        {
            new int MyProperty { get; set; }
            new void Method();
        }


        // all types memberss
        public interface ParentStruct
        {
            int MyProperty { get; set; }
        }
        public struct CTest : ParentStruct
        {
            new int x;
            static int y;
            public new int MyProperty { get; set; }
            static int StaticProp { get; set; }
            public abstract void Method();
            static void StaticMethod() { }

            delegate void TestDeleg();
            struct SStruct { }
            enum EEnum { }
            interface ITest { }

            class CClass { }
            static class StaticCClass { }
            sealed class SealedClass { }
            abstract class AbstractClass { }
        }

        class T
        {
            public int x;
            public virtual int MyProperty { get; set; }
        }
        abstract class X : T
        {
            new int x;
            static int y;
            public override int MyProperty { get; set; }
            static int StaticProp { get; set; }
            public abstract void Method();
            public virtual void StaticMethod() { }

            delegate void TestDeleg();
            struct SStruct { }
            enum EEnum { }
            interface ITest { }

            class CClass { }
            static class StaticCClass { }
            sealed class SealedClass { }
            abstract class AbstractClass { }
        }
    }

    public enum En { a, b, c }

    public struct Str
    {
        int x;
    }

    public abstract class A
    {
        public delegate void DelegateClassTest();
        public virtual void T() { }
        public abstract void X();
        public En Y() { return En.a; }
        public delegate void Test();
    }

    public sealed class B : A
    {
        public sealed override void T() { }

        public override void X()
        {

            throw new NotImplementedException();
        }
    }

    public abstract class C
    {

    }
}
";

        #endregion Constants

        static void Main(string[] args)
        {
            ISyntaxTreeBuilder syntaxTreeBuilder = new SyntaxTreeBuilder();
            var syntaxTree = syntaxTreeBuilder.BuildSyntaxTree(Sc2);//ScMinimum);

            var csharpOntology = new Ontology(name: "C# OOP Ontology", prefix: "cscro", fileName: "cscro.owl");
            var knowledgeBase = new KnowledgeBase("Main KB", csharpOntology);

            var graph1 = knowledgeBase.CreateGraph("graph2");
            var sourceCodeRdf = knowledgeBase.AddDataToGraph("graph2", "test2.cs", syntaxTree);

            //Assume that the Graph to be saved has already been loaded into a variable g
            RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();

            //Save to a File
            rdfxmlwriter.Save(sourceCodeRdf, "graph2.rdf");
        }
    }
}
