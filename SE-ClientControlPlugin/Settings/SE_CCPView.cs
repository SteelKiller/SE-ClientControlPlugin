using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torch;
using Torch.Views;

namespace SE_ClientControlPlugin.Settings
{
    class SE_CCPView:ViewModel
    {
        private string _name="nameMy";

        [Display(Name = "Name", Description = "Name of the limit. This helps with some of the commands")]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
                //Save();
            }
        }
    }
}
