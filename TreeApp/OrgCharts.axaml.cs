using Avalonia.Controls;
using Avalonia.Interactivity;
using Trees;

namespace TreeApp;

public partial class OrgCharts : Window
{
    public OrgCharts()
    {
        InitializeComponent();
    }
    
    private void BackButton_OnClick(object sender, RoutedEventArgs e)
    {
        var mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }


    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        //DrawBinaryNode();
        //DrawNaryNode();
        BuildGeneriGloopTree();
    }
    
    public void DrawNaryNode()
    {
        // Build a test tree.
        // A
        //         |
        //     +---+---+
        // B   C   D
        //     |       |
        //    +-+      +
        // E F      G
        //    |        |
        //    +      +-+-+
        // H      I J K
        var node_a = new NaryNode<string>("A");
       var node_b = new NaryNode<string>("B");
        var node_c = new NaryNode<string>("C");
        var node_d = new NaryNode<string>("D");
        var node_e = new NaryNode<string>("E");
        var node_f = new NaryNode<string>("F");
        var node_g = new NaryNode<string>("G");
        var node_h = new NaryNode<string>("H");
        var node_i = new NaryNode<string>("I");
        var node_j = new NaryNode<string>("J");
        var node_k = new NaryNode<string>("K");

        node_a.AddChild(node_b);
        node_a.AddChild(node_c);
        node_a.AddChild(node_d);
        node_b.AddChild(node_e);
        node_b.AddChild(node_f);
        node_d.AddChild(node_g);
        node_e.AddChild(node_h);
        node_g.AddChild(node_i);
        node_g.AddChild(node_j);
        node_g.AddChild(node_k);

        // Draw the tree.
        node_a.ArrangeAndDrawSubtree(MainCanvas, 10, 10);
    }
    
     // Build a test tree.
     private void BuildGeneriGloopTree()
     {
         // Build the top levels.
         NaryNode<string> generi_gloop = new NaryNode<string>("GeneriGloop");
         NaryNode<string> r_d = new NaryNode<string>("R & D");
         NaryNode<string> sales = new NaryNode<string>("Sales");
         NaryNode<string> professional_services = new NaryNode<string>("Professional\nServices");
         NaryNode<string> applied = new NaryNode<string>("Applied");
         NaryNode<string> basic = new NaryNode<string>("Basic");
         NaryNode<string> advanced = new NaryNode<string>("Advanced");
         NaryNode<string> sci_fi = new NaryNode<string>("Sci Fi");
         NaryNode<string> inside_sales = new NaryNode<string>("Inside\nSales");
         NaryNode<string> outside_sales = new NaryNode<string>("Outside\nSales");
         NaryNode<string> b2b = new NaryNode<string>("B2B");
         NaryNode<string> consumer = new NaryNode<string>("Consumer");
         NaryNode<string> account_management = new NaryNode<string>("Account\nManagement");
         NaryNode<string> hr = new NaryNode<string>("HR");
         NaryNode<string> accounting = new NaryNode<string>("Accounting");
         NaryNode<string> legal = new NaryNode<string>("Legal");

         generi_gloop.AddChild(r_d);
         generi_gloop.AddChild(sales);
         generi_gloop.AddChild(professional_services);

         professional_services.AddChild(hr);
         professional_services.AddChild(accounting);
         professional_services.AddChild(legal);

         if (true)
         {
             NaryNode<string> training = new NaryNode<string>("Training");
             NaryNode<string> hiring = new NaryNode<string>("Hiring");
             NaryNode<string> equity = new NaryNode<string>("Equity");
             NaryNode<string> discipline = new NaryNode<string>("Discipline");
             NaryNode<string> payroll = new NaryNode<string>("Payroll");
             NaryNode<string> billing = new NaryNode<string>("Billing");
             NaryNode<string> reporting = new NaryNode<string>("Reporting");
             NaryNode<string> opacity = new NaryNode<string>("Opacity");
             NaryNode<string> compliance = new NaryNode<string>("Compliance");
             NaryNode<string> progress_prevention = new NaryNode<string>("Progress\nPrevention");
             NaryNode<string> bail_services = new NaryNode<string>("Bail\nServices");

             r_d.AddChild(applied);
             r_d.AddChild(basic);
             r_d.AddChild(advanced);
             r_d.AddChild(sci_fi);

             sales.AddChild(inside_sales);
             sales.AddChild(outside_sales);
             sales.AddChild(b2b);
             sales.AddChild(consumer);
             sales.AddChild(account_management);

             hr.AddChild(training);
             hr.AddChild(hiring);
             hr.AddChild(equity);
             hr.AddChild(discipline);

             accounting.AddChild(payroll);
             accounting.AddChild(billing);
             accounting.AddChild(reporting);
             accounting.AddChild(opacity);

             legal.AddChild(compliance);
             legal.AddChild(progress_prevention);
             legal.AddChild(bail_services);
         }

         generi_gloop.ArrangeAndDrawSubtree(MainCanvas, 10, 10);
     }

}