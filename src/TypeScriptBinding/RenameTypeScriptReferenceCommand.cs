﻿// 
// RenameTypeScriptReferenceCommand.cs
// 
// Author:
//   Matt Ward <ward.matt@gmail.com>
// 
// Copyright (C) 2013 Matthew Ward
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;

using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Editor;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpDevelop.Refactoring;

namespace ICSharpCode.TypeScriptBinding
{
	public class RenameTypeScriptReferenceCommand : AbstractCommand
	{
		public override void Run()
		{
			var editorProvider = WorkbenchSingleton.Workbench.ActiveViewContent as ITextEditorProvider;
			if (editorProvider != null) {
				List<Reference> references = TypeScriptCodeCompletionBinding.GetReferences(editorProvider.TextEditor);
				
				if (references.Count == 0) {
					ShowUnknownReferenceError();
				} else {
					RenameAllReferences(references);
				}
			}
		}
		
		static void ShowUnknownReferenceError()
		{
			MessageService.ShowMessage("${res:SharpDevelop.Refactoring.CannotRenameElement}");
		}
		
		void RenameAllReferences(List<Reference> references)
		{
			string name = references.First().Expression;
			string newName = GetNewName(name);
			if (ShouldRenameReference(newName, name)) {
				FindReferencesAndRenameHelper.RenameReferences(references, newName);
			}
		}
		
		string GetNewName(string name)
		{
			return MessageService.ShowInputBox(
				"${res:SharpDevelop.Refactoring.Rename}",
				"${res:SharpDevelop.Refactoring.RenameClassText}",
				name);
		}
		
		bool ShouldRenameReference(string newName, string name)
		{
			return (newName != null) && (newName != name);
		}
	}
}
