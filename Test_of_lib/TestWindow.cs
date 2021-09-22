using System.Threading;
using Tools;
using Tools.Window;
using Tools.Window.Components;

namespace Test_of_lib
{
    class TestWindow : Window
    {
        public TestWindow()
        {
            Height = 400;
            Width = 1000;
            Title = "Hello, world!";
            this.components.Add(new Button());
            InitializeComponents();

            Showed += TestWindow_Showed;
        }

        private void TestWindow_Showed(object sender, ShowedHandlerArgs e)
        {

        }
    }
}
