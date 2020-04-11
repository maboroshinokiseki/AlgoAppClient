using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlgoApp.Utilities
{
    class UIUtilities
    {
        public static async Task<string> DisplayPromptAsync(Page page, string title, string message, string errorMessage, string placeholder = null, string initialValue = "")
        {
            string inputText;
            while (true)
            {
                inputText = await page.DisplayPromptAsync(title, message, "確認", "取消", placeholder, initialValue: initialValue);
                if (string.IsNullOrWhiteSpace(inputText))
                {
                    if (inputText != null)
                    {
                        await page.DisplayAlert("错误", errorMessage, "确认");
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    break;
                }
            }

            return inputText;
        }
    }
}
