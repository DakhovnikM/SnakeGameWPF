using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameWPF.ViewModels
{
    class DialogWindowViewModel : BaseViewModel
    {
        public string Content { get; set; } = "У вас закончились жизни !!!\nНачать заново?";

        public DialogWindowViewModel()
        {
            
        }

    }
}
