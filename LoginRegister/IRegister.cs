using System.ComponentModel;

namespace ProDiaryApplication
{
    public interface IRegister
    {
        event PropertyChangedEventHandler? PropertyChanged;

        void InitializeComponent();
    }
}