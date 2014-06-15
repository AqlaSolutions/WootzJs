﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WootzJs.Compiler
{
    public class AsyncStateGenerator : StateGenerator
    {
        public AsyncStateGenerator(Compilation compilation, MethodDeclarationSyntax node) : base(compilation, node)
        {
        }

        protected override StatementSyntax ReturnOutOfState()
        {
            return Cs.Return();
        }

        public override void VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            currentState.Add(StateMachineThisFixer.Fix(node));
        }
    }
}