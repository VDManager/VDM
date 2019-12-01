using System.Windows.Controls;
using VDM.GUI.ViewModels.Factories;


namespace VDM.GUI.Views
{
    /// <summary>
    /// Interaction logic for VDPDetailsView.xaml
    /// </summary>
    public partial class VDPDetailsView : UserControl
    {
        public VDPDetailsView(VDPDetailsViewModelFactory factory)
        {
            this.DataContext = factory.Build();
            InitializeComponent();
        }
    }
}
