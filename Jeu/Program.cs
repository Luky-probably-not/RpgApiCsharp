using Modeles.Character.Ennemie;
using Jeu;
using Modeles;
using Modeles.Capacites;

//GameManager.Instance.Debut(3).GetAwaiter().GetResult();

var frappe = new Sacrifice();
Console.WriteLine(frappe.Description.Length);
frappe.SplitEveryNth(10).ForEach(Console.WriteLine);