using AlgoApp.Views.Teacher;
using System.Threading.Tasks;

namespace AlgoApp.Extensions
{
    public static class PageExtensions
    {
        public static async Task<string> DisplayPrompt(this ClassRoomListPage page, string title, string message, string errorMessage, string placeholder = null, string initialValue = "")
        {

            string inputText;
            while (true)
            {
                inputText = await page.DisplayPromptAsync(title, message, "确认", "取消", placeholder, initialValue: initialValue);
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
