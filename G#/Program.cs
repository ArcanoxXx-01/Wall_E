// namespace Wall_E
// {
//     public class Program
//     {
// public static void Main(string[] args)
// {
//     Functions.Fun();
//     string input = Console.ReadLine();

//     Tokenizer tokenizer = new Tokenizer(input);

//     if (tokenizer.errores.Count != 0)
//     {
//         foreach (ERROR x in tokenizer.errores)
//         {
//             System.Console.WriteLine(x.Type + " " + x.Mensaje);
//         }
//     }

//     else
//     {
//         // foreach (Token x in tokenizer.tokens)
//         // {
//         //    System.Console.WriteLine(x.type + " " + x.value);
//         // }

//         try
//         {
//             Parser parser = new Parser(tokenizer.tokens);
//             // System.Console.WriteLine(parser.exprs.Count);
//             // if(parser.exprs[0] is Expression.Sequence) 
//             // {
//             //    System.Console.WriteLine(true);
//             // }

//             foreach (Expression expr in parser.exprs)
//             {
//                 System.Console.WriteLine(expr.GetType());
//             }
//         }

//         catch (ERROR)
//         {
//             return;
//         }
//     }

//     // Point p1 = new Point(57, 8);
//     // Point p2 = new Point(16, -19);
//     // Point p3 = new Point(3, 3);
//     // Line l1 = new Line(p1, p2);
//     // Circle c1 = new Circle(p1, 30);
//     // Circle c2 = new Circle(p2, 20);
//     // Segment s1 = new Segment(p1, p3);
//     //  foreach (Point x in c1.Intersect(c2))
//     //  {
//     //     System.Console.WriteLine(x.x + " ; " + x.y);
//     //  }

// }
//     }
// }