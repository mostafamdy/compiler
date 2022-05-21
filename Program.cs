using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace compiler_project
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Scanner scanner = new Scanner();
            string path = "E:\\New folder 15\\compiler\\compiler_project\\compiler_project\\compiler_project.txt";
            scanner.read_file(path);
        }

    }
    class Scanner
    {
        Dictionary<string, TokenType> token_keyword = new Dictionary<string, TokenType>();
        
        Dictionary<int, List<string>> program = new Dictionary<int, List<string>>();
        
        char[] end_of_statement = new char[] { ';', '\n' };

        int num_of_errors = 0;
        void init()
        {
            token_keyword.Add("if", TokenType.Condition);
            token_keyword.Add("else", TokenType.Condition);
            token_keyword.Add("Iow", TokenType.Integer);
            token_keyword.Add("SIow", TokenType.SInteger);
            token_keyword.Add("Chlo", TokenType.Character);
            token_keyword.Add("Chain", TokenType.String);
            token_keyword.Add("Iowf", TokenType.Float);
            token_keyword.Add("SIowf", TokenType.SFloat);
            token_keyword.Add("Worthless", TokenType.Void);
            token_keyword.Add("Loopwhen", TokenType.Loop);
            token_keyword.Add("Iteratewhen", TokenType.Loop);
            token_keyword.Add("Turnback", TokenType.Return);
            token_keyword.Add("Stop", TokenType.Break);
            token_keyword.Add("Loli", TokenType.Struct);
            token_keyword.Add("+", TokenType.Arithmetic_operation);
            token_keyword.Add("-", TokenType.Arithmetic_operation);
            token_keyword.Add("*", TokenType.Arithmetic_operation);
            token_keyword.Add("/", TokenType.Arithmetic_operation);
            token_keyword.Add("&&", TokenType.Logic_operators);
            token_keyword.Add("||", TokenType.Logic_operators);
            token_keyword.Add("~", TokenType.Logic_operators);
            token_keyword.Add("==", TokenType.relational_operators);
            token_keyword.Add("<", TokenType.relational_operators);
            token_keyword.Add(">", TokenType.relational_operators);
            token_keyword.Add("!=", TokenType.relational_operators);
            token_keyword.Add("<=", TokenType.relational_operators);
            token_keyword.Add(">=", TokenType.relational_operators);
            token_keyword.Add("=", TokenType.Assignment_operator);
            token_keyword.Add("->", TokenType.Access_Operator);
            token_keyword.Add("{", TokenType.Braces);
            token_keyword.Add("}", TokenType.Braces);
            token_keyword.Add("[", TokenType.Braces);
            token_keyword.Add("]", TokenType.Braces);
            token_keyword.Add("(", TokenType.Braces);
            token_keyword.Add(")", TokenType.Braces);
            token_keyword.Add("“,’", TokenType.Quotation_Mark);
            token_keyword.Add("Include", TokenType.Inclusion);
        }

        List<string> tokens;
        string last = "_@_";
        List<string> scanner_result=new List<string>();
        public void read_file(string path)
        {
            init();
            StreamReader reader = new StreamReader(path);
            string all_data = reader.ReadToEnd();
            string[] statement = all_data.Split(end_of_statement);
            
            // toknize data
            for (int i = 0; i < statement.Length - 1; i++)
            {
                tokens = toknize(statement[i]);
                List<string> clear = new List<string>();
                for(int K = 0; K < tokens.Count; K++)
                {
                    if (tokens[K] != " " && tokens[K] != last)
                    {
                        clear.Add(tokens[K]);
                    }
                }
                program.Add(i, clear);
                for (int k = 0; k < tokens.Count; k++)
                {

                    if (is_coment(k))
                    {
                        
                       // Console.WriteLine("\nis comment");

                        for (int j = k; j < tokens.Count; j++)
                        {
                            if (is_coment_ended(j))
                            {
                                k = j + 1;
                            }
                        }
                    }

                    else if (is_constant(tokens[k]))
                    {
                        scanner_result.Add("Line " + i.ToString() + " Token Text: " + tokens[k] + " token type constatnt");
                        //Console.WriteLine("\ntoken Text" + " " + tokens[k] + " token type " + "constatnt");
                    }

                    else if (token_keyword.ContainsKey(tokens[k]))
                    {
                        scanner_result.Add("Line " + i.ToString() + " Token Text: " + tokens[k] + " token type "+ token_keyword[tokens[k]]);
                        //Console.WriteLine("\ntoken Text" + " " + tokens[k] + " token type " + token_keyword[tokens[k]]);
                    }
                    else if (is_idintfier(tokens[k]))
                    {
                        scanner_result.Add("Line " + i.ToString() + " Token Text: " + tokens[k] + " token type Identifier");
                        //Console.WriteLine("\ntoken Text" + " " + tokens[k] + " token type " + "Identifier");
                    }


                    else
                    {
                        if (tokens[k] != " " && tokens[k] != last)
                        {
                            scanner_result.Add("Line " + i.ToString() + " Error in Token Text: " + tokens[k]);
                            num_of_errors += 1;
                            //Console.WriteLine("Error " + tokens[k]);
                        }
                    }
                }
            }
            scanner_result.Add("Total NO of errors: " +num_of_errors.ToString());
            for(int i = 0; i < scanner_result.Count; i++)
            {
                Console.WriteLine(scanner_result[i]);
            }
            //parser 
            new Parser(program);
        }
        bool is_coment(int indx)
        {

            if (tokens[indx] == "/" && tokens[indx + 1] == "$")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool is_coment_ended(int indx)
        {
            if (tokens[indx] == "$" && tokens[indx + 1] == "/")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool is_idintfier(string data)
        {
            if (Char.IsDigit(data[0]) || !(data != " " && data != last))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        bool is_constant(string data)
        {
            int outd;
            return int.TryParse(data, out outd);
        }

        List<string> toknize(string sentene)
        {
            //char[]
            string[] toknize_by = new string[] {
                //'$',' ','{', '}','(',')','\n','<','>','!','=','$','|','~','+','-','*','/','[',']' };
                "$"," ","{", "}","(",")","\n","<",">","!","=","$","|","~","+","-","*","/","[","]","," };

            //List<string> d = new List<string>();
            string all_d = sentene;
            string[] words = all_d.Split(toknize_by, StringSplitOptions.RemoveEmptyEntries);

            List<string> tokens = new List<string>();
            int last_index = 0;
            foreach (var word in words)
            {
                int current_index = all_d.IndexOf(word, last_index);
                for (int i = last_index; i < current_index; i++)
                {
                    tokens.Add(all_d[i].ToString());
                }

                tokens.Add(word);

                last_index = current_index + word.Length;
            }
            /*
            foreach (var token in tokens)
            {
                Console.WriteLine($"<{token}>");
            }*/
            if (last == "_@_")
            {
                last = tokens[tokens.Count - 1];
            }

            //Console.WriteLine("_______________________________________________________");
            return tokens;
        }
    }


    public class Parser{
        public Parser(Dictionary<int, List<string>> program)
        {
            this.program = program;
            tst();

        }

        Dictionary<int, List<string>> program;
        string last = "";
        void tst()
        {/*
          * 
            for (int i = 0; i < program.Count; i++)
            {
                Console.WriteLine(i.ToString());
                for (int t = 0; t < program[i].Count; t++)
                {
                    Console.Write(program[i][t]);
                }
                Console.WriteLine("\n");
            }*/
            Console.WriteLine("in test");
            //is_fun_declaration(program, 1);
            Console.WriteLine(is_var_declaration(program[2]));

        }        
        //fun declaration = type-specifier + id +(+param or params or nothing+) +  compound-stmt
        //type-specifier=  Iow | SIow | Chlo | Chain | Iowf | SIowf | Worthless

        // id =is_idintfier(data)


        //Comment
        //

        void is_declartion(Dictionary<int, List<string>> program,int start_indx)
        {
            List<string> tokens = program[start_indx];
            is_var_declaration(tokens);
            is_fun_declaration(program,start_indx);
            
        }
        void is_fun_declaration(Dictionary<int, List<string>> program, int start_indx)
        {
            List<string> tokens = program[start_indx];
            //Console.WriteLine("tokens");
            /*for (int t = 0; t < tokens.Count; t++)
            {
                Console.Write(tokens[t]+" ");
            }*/
           
            if (is_type_specifier(tokens[0]) && is_idintfier(tokens[1]) && is_open_brace(tokens[2])&&is_param_list(tokens, 4))
            {
                Console.WriteLine("is a function");
            }

        }

        //  get back
        (bool,int) is_compound_stmt(Dictionary<int, List<string>> program, int start_indx)
        {
            if (program[start_indx][0] != "{")
            {
                return (false,start_indx);
            }
            else
            {
                (bool _,int indx)=is_local_declarations(program, start_indx);

                return(true,1);
            }
        }
        
        (bool,int) is_local_declarations(Dictionary<int, List<string>> program, int start_indx)
        {
            if (is_var_declaration(program[start_indx])==false)
            {
                return (false, start_indx);
            }
            else
            {
                int last_indx = start_indx;
                for (int i = start_indx; i < program.Count; i++)
                {
                    // 
                    if (is_var_declaration(program[i]))
                    {
                        continue;
                    }
                    else
                    {
                        last_indx = i;
                        break;
                    }
                }
                return (true, last_indx);
            }

        }

        bool is_var_declaration(List<string> tokens)
        {
            if (is_type_specifier(tokens[0])&& is_idintfier(tokens[1]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool is_type_specifier(string data)
        {
            if (data== "Iow" || data == "SIow" || data == "Chlo" || data == "Chain" || data == "Iowf" || data == "SIowf" || data == "Worthless")
            {
                
                return true;
            }
            else
            {
                return false;
            }
        }

        bool is_idintfier(string data)
        {
            if (Char.IsDigit(data[0]) || !(data != " " && data != last))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        bool is_open_brace(string data)
        {
            return data == "(";
        }

        bool is_close_brace(string data)
        {
            return data == ")";
        }

        bool is_param(List<string> all_tokens,int indx)
        {

            return (is_type_specifier(all_tokens[indx - 1])&&is_idintfier(all_tokens[indx]));
        }

        bool is_param_list(List<string> all_tokens,int indx)
        {

            if (is_close_brace(all_tokens[indx-1]))
            {
                return true;
            }
            
            if (all_tokens[indx + 1] == ",")
            {
                //Console.WriteLine("HERE");
                int end_indx = -1;
                for (int i=indx;i< all_tokens.Count; i+=3)
                {
                    //Console.WriteLine("i "+i.ToString()+all_tokens[i]);
                    if (is_param(all_tokens, i)==false)
                    {
                        break;
                    }
                    if (all_tokens[i + 1] != ",")
                    {
                        if (is_close_brace(all_tokens[i + 1]))
                        {
                            end_indx = i;
                        }
                        break;
                    }
                }
                
                if (end_indx==-1)
                {
                    return false;
                }


                return true;
            }
            else
            {
                Console.WriteLine("one parm");
                return is_param(all_tokens, indx);
            }
            
        }

        // expression
        bool is_expression(List<string> tokens)
        {
            // id=id
            if (is_idintfier(tokens[0])&& tokens[1] == "=" && is_idintfier(tokens[2]))
            {
                return true;   
            }
            else if(is_idintfier(tokens[0]) && tokens[1] == "=" && is_simple_expression(tokens[2]))
            {
                return true;
            }
            else if (is_simple_expression(tokens[0]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool is_simple_expression(string data)
        {
            //if additive relop additive 
            //or additive

            return true;
        }

        bool additive_expression()
        {
            return false;
            //additive_expression addop term
            // term

        }
        bool is_term()
        {
            return false;
            //term mulop factor 
            //factor

        }
        bool is_factor()
        {
            return false;
        }
        bool is_add_op(string data)
        {
            return data == "+" || data == "-";
        }
        bool is_mul_op(string data)
        {
            return data == "*" || data == "/";
        }
        bool is_rel_op(string data)
        {
            return data == "<=" || data == "<" || data == ">" || data == ">=" || data == "==" || data == "!=" || data == "&&" || data == "||";

        }

        bool is_call(List<string> tokens)
        {
            is_idintfier(tokens[0]);
            is_open_brace(tokens[1]);
            is_expression(tokens);
            return true;
        }
    }


    public enum TokenType
    {
        Condition,
        Integer,
        SInteger,
        Character,
        String,
        Float,
        SFloat,
        Void,
        Loop,
        Return,
        Break,
        Struct,
        Logic_operators,
        Arithmetic_operation,
        relational_operators,
        Assignment_operator,
        Access_Operator,
        Braces,
        Constant,
        Quotation_Mark,
        Inclusion,
    } 
}
