using System;
using System.Collections;
using System.Text.RegularExpressions;
using Discuz.Common;
using Discuz.Space.Provider;

namespace Discuz.Space.Utilities
{
	/// <summary>
	/// TemplateEngine 的摘要说明。
	/// </summary>
	public class TemplateEngine
	{
		public TemplateEngine()
		{
		}

		private Hashtable _context = new Hashtable();
		private string _template = "";
		private string _templateRoot = string.Empty;

		public string TemplateRoot
		{
			get { return _templateRoot; }
			set
			{
				if (!value.EndsWith("\\"))
					_templateRoot = value + "\\";
				else
					_templateRoot = value;
			}
		}

		#region context

		public void Put(String key, Object value_Renamed)
		{
			if (_context[key] != null)
			{
				_context[key] = value_Renamed;
			}
			else
			{
				_context.Add(key, value_Renamed);
			}
		}

		public Object Get(String key)
		{
			if (_context[key] != null)
			{
				return _context[key];
			}
			return null;
		}

		public void Remove(String key)
		{
			if (_context[key] != null)
			{
				_context.Remove(key);
			}
		}

		public Boolean ContainsKey(String key)
		{
			if (_context[key] != null)
			{
				return true;
			}
			return false;
		}

		#endregion

		public void Init(string template, string templateRoot)
		{
			this._template = template;
			this.TemplateRoot = templateRoot;
		}

		public void Init(string template)
		{
			this.Init(template, string.Empty);
		}


		/// <summary>
		/// 合并模板
		/// </summary>
		/// <returns></returns>
		public string MergeTemplate()
		{
			ClearNote();
			ParseObjectProperty();
			ParseSubTemplate();
			ParseLogic();
			foreach (DictionaryEntry de in _context)
			{
				_template = _template.Replace("${" + de.Key.ToString() + "}", de.Value.ToString());
			}

			return _template;
		}

		~TemplateEngine()
		{
			_context = null;
			_template = string.Empty;
		}

		public string ParseSet()
		{
			return null;
		}

		public string ParseObjectProperty()
		{
			Regex r = new Regex(@"\$\{([\w]+?)\.([\w]+?)\}");
			MatchCollection ms = r.Matches(_template);

			foreach (Match m in ms)
			{
				_template = _template.Replace(m.Value, ParseValue(m.Value).ToString());
			}
			return string.Empty;
		}

		public string ParseSubTemplate()
		{
			if (this._templateRoot == string.Empty)
				return string.Empty;

			Regex r = new Regex(@"\#parse( )*\(([\s\S]+?)\)");
			MatchCollection ms = r.Matches(_template);

			for (int i = 0; i < ms.Count; i++)
			{
				string codeBlock = ms[i].Groups[0].Value;
				string filename = this._templateRoot + ParseValue(ms[i].Groups[2].Value);
				string content = StaticFileProvider.GetContent(filename);
				TemplateEngine te = new TemplateEngine();
				te.Init(content, this._templateRoot);
				te._context = this._context;
				content = te.MergeTemplate();
				this._template = this._template.Replace(codeBlock, content);
			}

			return string.Empty;
		}

		private string ParseLogic()
		{
			#region 解析if语句

			string[] ifstatements = GetIfStatements();

			foreach (string s in ifstatements)
			{
				IfStatement ifs = new IfStatement(s);
				if (ParseExpression(ifs.Expression))
				{
					//表达式成立
					_template = _template.Replace(s, ifs.Yes);
				}
				else
				{
					//表达式不成立
					_template = _template.Replace(s, ifs.No);
				}
			}

			#endregion

			return string.Empty;
		}

		/// <summary>
		/// 清除模板中的注释
		/// </summary>
		/// <returns></returns>
		private void ClearNote()
		{
			Regex r = new Regex(@"\#\*([\s\S]+?)\*\#"); //多行注释
			_template = r.Replace(_template, string.Empty);
		}

		private bool ParseExpression(string expression)
		{
			//二目运算符
			Regex r = new Regex("([=|>|<|!]=|>|<)");
			string[] result = r.Split(expression);

			if (result.Length < 3)
			{
				//单目运算
				if (result[0].StartsWith("!"))
					return !Utils.StrToBool(ParseValue(result[0].Substring(1)), true);
				else
					return Utils.StrToBool(ParseValue(result[0]), false);
			}
			else
			{
				//二目运算
				string oper = result[1];
				object left = ParseValue(result[0]);
				object right = ParseValue(result[2]);

				if (left.GetType() != right.GetType())
					_template = "表达式两边类型不同 " + expression;
				switch (oper)
				{
					case ">=":
						if (left.GetType() == typeof (int))
							return Utils.StrToInt(left, 0) >= Utils.StrToInt(right, 0);
						else
							_template = "不是整型不能 >= 比较 " + expression;
						break;
					case "<=":
						if (left.GetType() == typeof (int))
							return Utils.StrToInt(left, 0) <= Utils.StrToInt(right, 0);
						else
							_template = "不是整型不能 <= 比较 " + expression;
						break;
					case "==":
						return left.ToString() == right.ToString();
					case "!=":
						return left.ToString() != right.ToString();
					case ">":
						if (left.GetType() == typeof (int))
							return Utils.StrToInt(left, 0) > Utils.StrToInt(right, 0);
						else
							_template = "不是整型不能 > 比较 " + expression;
						break;
					case "<":
						if (left.GetType() == typeof (int))
							return Utils.StrToInt(left, 0) < Utils.StrToInt(right, 0);
						else
							_template = "不是整型不能 < 比较 " + expression;
						break;
				}
			}
			return false;
		}

		private object ParseValue(string value)
		{
			value = value.Trim();
			object obj = null;
			if (value.EndsWith("}"))
			{
				//以}结尾，说明是变量
				value = value.Remove(value.LastIndexOf("}"), 1);
				value = value.Remove(0, 2);

				if (value.IndexOf('.') > -1)
				{
					//变量中带.说明是调用对象的属性
					string[] objProperty = value.Split('.'); //拆分,[0]为对象名(即_context中的key),[1]为对应对象的属性的值

					obj = _context[objProperty[0]];
					if (obj == null)
					{
						_template = "变量" + objProperty[0] + "为空或者不存在";
						obj = string.Empty;
					}
					else
					{
						//通过反射取值
						obj = obj.GetType().GetProperty(objProperty[1]).GetValue(obj, null).ToString();
						if (obj == null)
							obj = string.Empty;
					}
				}
				else
				{
					obj = _context[value];
					if (obj == null)
					{
						_template = "变量 " + value + " 不存在";
						obj = string.Empty;
					}
				}
			}
			else if (Utils.IsNumeric(value))
			{
				//是数字常量
				obj = Utils.StrToInt(value, 0);
			}
			else if (value.StartsWith("\"") && value.EndsWith("\""))
			{
				//最后一种可能,字符串常量
				obj = value.Remove(0, 1);
				obj = obj.ToString().Remove(obj.ToString().LastIndexOf("\""), 1);
			}
			else
			{
				_template = "无法识别的变量: " + value;
				obj = string.Empty;
			}

			return obj;
		}

		private string[] GetIfStatements()
		{
			Regex r = new Regex(@"#if([\s\S]+?)#end");
			MatchCollection ms = r.Matches(_template);

			string[] result = new string[ms.Count];

			for (int i = 0; i < ms.Count; i++)
			{
				result[i] = ms[i].Captures[0].Value;
			}

			return result;
		}
	}

	public class IfStatement
	{
		public IfStatement()
		{
		}

		public IfStatement(string str)
		{
			this._statement = str;
			if (this._statement.IndexOf("#else") > -1)
				this._hasElse = true;

			this._expression = GetExpression();
			this._statement = this._statement.Replace(string.Format("({0})", this._expression), string.Empty);
			this._yes = GetYesStatement();
			this._no = GetNoStatement();
		}

		private string _statement = string.Empty;
		private bool _hasElse = false;
		private string _yes = string.Empty;
		private string _no = string.Empty;
		private string _expression = string.Empty;

		public bool HasElse
		{
			get { return _hasElse; }
			set { _hasElse = value; }
		}

		public string Yes
		{
			get { return _yes; }
			set { _yes = value; }
		}

		public string No
		{
			get { return _no; }
			set { _no = value; }
		}

		public string Expression
		{
			get { return _expression; }
			set { _expression = value; }
		}

		private string GetExpression()
		{
			Regex r = new Regex(@"\(([\s\S]+?)\)");
			Match m = r.Match(this._statement);

			if (m.Success)
				return m.Groups[1].Value;

			return string.Empty;
		}

		private string GetYesStatement()
		{
			Regex r = new Regex(@"\#if([\s\S]+?)\#end");
			if (_hasElse)
			{
				r = new Regex(@"\#if([\s\S]+?)\#else");
			}
			Match m = r.Match(this._statement);

			if (m.Success)
				return m.Groups[1].Value;
			return string.Empty;
		}

		private string GetNoStatement()
		{
			if (_hasElse)
			{
				Regex r = new Regex(@"\#else([\s\S]+?)\#end");
				Match m = r.Match(this._statement);

				if (m.Success)
					return m.Groups[1].Value;
			}
			return string.Empty;
		}
	}
}