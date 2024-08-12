using CommunityToolkit.Mvvm.ComponentModel;
using Infinity_Scrolling_MAUI.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Infinity_Scrolling_MAUI
{
    public partial class MainViewModel : ObservableObject
    {
        private int lastLoadedIndex = 0;
        private const int loadBatchSize = 5;
        private int sourceSize = 0;

        [ObservableProperty]
        private bool isLoading;

        public ObservableCollection<BlogPost> Blogs { get; set; }

        public ICommand LoadMoreCommand { get; set; }
        public ICommand ShareCommand { get; set; }
        public ICommand OpenBlogCommand { get; set; }

        public MainViewModel()
        {
            Blogs = new ObservableCollection<BlogPost>();
            sourceSize = DataStorage.GetTotalCount();
            LoadBatch();
            LoadMoreCommand = new Command(LoadMore, CanLoadMore);
            ShareCommand = new Command<BlogPost>(ShareBlog);
            OpenBlogCommand = new Command<BlogPost>(OpenBlog);
        }

        private async void LoadBatch()
        {
            IsLoading = true;
            IEnumerable<BlogPost> newBlogsBatch = null;
            await Task.Run(() =>
            {
                Thread.Sleep(1000);
                newBlogsBatch = DataStorage.GetBlogs(lastLoadedIndex, loadBatchSize);
            });

            lastLoadedIndex += loadBatchSize;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                foreach (var blog in newBlogsBatch)
                {
                    Blogs.Add(blog);
                }
            });

            IsLoading = false;
            ((Command)LoadMoreCommand).ChangeCanExecute();
        }

        private void LoadMore()
        {
            LoadBatch();
        }

        private bool CanLoadMore()
        {
            return Blogs.Count < sourceSize && !IsLoading;
        }

        private async void ShareBlog(BlogPost blog)
        {
            await Share.Default.RequestAsync(new ShareTextRequest
            {
                Text = blog.Url,
                Title = "Share the Blog With your Friends"
            });
        }

        private async void OpenBlog(BlogPost blog)
        {
            try
            {
                await Browser.Default.OpenAsync(new Uri(blog.Url), BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Error", "Couldn't open the URL", "OK");
            }
        }
    }

    public static class DataStorage
    {
        static DataStorage()
        {
            allBlogs = CreateBlogs();
        }

        public static IEnumerable<BlogPost> GetBlogs(int startIndex, int batchSize)
        {
            return allBlogs.Skip(startIndex).Take(batchSize);
        }

        public static int GetTotalCount()
        {
            return allBlogs.Count;
        }

        private static readonly List<BlogPost> allBlogs;

        private static List<BlogPost> CreateBlogs()
        {
            return new List<BlogPost>
            {
                new BlogPost(1, "DevExtreme Roadmap (Angular, React, Vue, jQuery)", "Vlada", new DateTime(2023,2,22), "devextreme_roadmap", "https://community.devexpress.com/blogs/javascript/archive/2023/02/22/devextreme-components-roadmap-2023-1.aspx"),
                new BlogPost(2, "Blazor Editors — Command Buttons", "Margarita", new DateTime(2023,2,22), "blazor_editors_buttons","https://community.devexpress.com/blogs/aspnet/archive/2023/02/22/Blazor-Editors-Command-Buttons-v22-2.aspx"),
                new BlogPost(3, "DevExpress Reports Roadmap (Survey Inside)", "Dmitry", new DateTime(2023,2,21), "reporting_roadmap", "https://community.devexpress.com/blogs/reporting/archive/2023/02/21/devexpress-reports-v23-1-june-2023-roadmap-survey-inside.aspx"),
                new BlogPost(4, "Announcing DevExpress Mobile UI for .NET MAUI", "Anthony", new DateTime(2023,2,21), "maui_release","https://community.devexpress.com/blogs/mobile/archive/2023/02/22/announcing-devexpress-mobile-ui-for-net-maui-v22-2.aspx"),
                new BlogPost(5, "Office File API & Office-Inspired UI Controls Roadmap (Survey Inside)", "Dmitry", new DateTime(2023,2,21), "office_roadmap","https://community.devexpress.com/blogs/office/archive/2023/02/21/office-file-api-office-inspired-ui-controls-v23-1-june-2023-roadmap-survey-inside.aspx"),
                new BlogPost(6, "DevExpress BI Dashboard Roadmap (Survey Inside)", "Dmitry", new DateTime(2023,2,21), "dashboard_roadmap","https://community.devexpress.com/blogs/analytics/archive/2023/02/21/devexpress-bi-dashboard-v23-1-june-2023-roadmap-survey-inside.aspx"),
                new BlogPost(7, "Reporting — Serial Shipping Container Code (SSCC-18): A Solution for Walmart's Packaging Needs", "Boris", new DateTime(2023,2,20), "reporting_serial_shipping","https://community.devexpress.com/blogs/reporting/archive/2023/02/20/reporting-serial-shipping-container-code-sscc-18-a-solution-for-walmart-39-s-packaging-needs.aspx"),
                new BlogPost(8, "DevExpress.Drawing Graphics Library — Update — Package Dependencies and Font Libraries", "Poline", new DateTime(2023,2,16), "devextreme__fonts","https://community.devexpress.com/blogs/news/archive/2023/02/16/devexpress-drawing-graphics-library-v22-2-4-update-package-dependencies-and-font-libraries.aspx"),
                new BlogPost(9, "DevExpress WinForms Roadmap", "Bogdan", new DateTime(2023,2,16), "winforms_roadmap","https://community.devexpress.com/blogs/winforms/archive/2023/02/16/devexpress-winforms-roadmap-23-1.aspx"),
                new BlogPost(10, "XAF Roadmap (Cross-Platform .NET App UI & Web API Service)", "Dennis", new DateTime(2023,2,9), "xaf_roadmap","https://community.devexpress.com/blogs/xaf/archive/2023/02/09/xaf-2023-1-roadmap-cross-platform-net-app-ui-and-web-api-service.aspx"),
                new BlogPost(11, "Blazor Reporting — Quick Start with New Project Templates", "Boris", new DateTime(2023,2,6), "blazor_reporting","https://community.devexpress.com/blogs/reporting/archive/2023/02/06/blazor-reporting-quick-start-with-new-project-templates.aspx"),
                new BlogPost(12, "Blazor Toolbar — Data Binding", "Elena", new DateTime(2023,2,1), "blazor_toolbar","https://community.devexpress.com/blogs/aspnet/archive/2023/02/01/Blazor-Toolbar-Data-Binding-_2800_v22.2_2900_.aspx"),
                new BlogPost(13, "9 Tips to Reduce WPF App Startup Time", "Andrey", new DateTime(2023,1,26), "wpf9tips","https://community.devexpress.com/blogs/wpf/archive/2023/01/26/9-tips-to-reduce-wpf-app-startup-time.aspx"),
                new BlogPost(14, ".NET MAUI Controls — Material Design 3", "Anthony", new DateTime(2023,1,25), "maui_md3","https://community.devexpress.com/blogs/mobile/archive/2023/01/25/net-maui-controls-material-design-3-v22-2.aspx"),
                new BlogPost(15, "eXpress Persistent Objects (XPO) — Roadmap 2023", "Dennis", new DateTime(2023,1,23), "xpo_roadmap","https://community.devexpress.com/blogs/xpo/archive/2023/01/23/xpo-2023-roadmap.aspx"),
                new BlogPost(16, "Save Time: The Easiest, No-Code Way to Visualize Your SQL Data (v23.1)", "Poline", new DateTime(2023,1,23), "analytics_easy","https://community.devexpress.com/blogs/analytics/archive/2023/01/23/save-time-the-easiest-no-code-way-to-visualize-your-sql-data-v23-1.aspx"),
                new BlogPost(17, "DevExpress WPF Controls — 2023 Roadmap", "Elena", new DateTime(2023,1,20), "wpf_roadmap","https://community.devexpress.com/blogs/wpf/archive/2023/01/20/devexpress-wpf-controls-2023-roadmap.aspx"),
                new BlogPost(18, "Blazor Data Grid — Master-Detail View", "Margarita", new DateTime(2023,1,19), "blazor_datagrid","https://community.devexpress.com/blogs/aspnet/archive/2023/01/19/Blazor-Data-Grid-Master-Detail-View.aspx"),
                new BlogPost(19, "WPF — 2023 Roadmap", "Elena", new DateTime(2023,1,17), "wpf_roadmap_23","https://community.devexpress.com/blogs/wpf/archive/2023/01/17/wpf-2023-roadmap.aspx"),
                new BlogPost(20, "Cross-Platform Products — 2023 Roadmap", "Dmitry", new DateTime(2023,1,16), "cross_platform_roadmap","https://community.devexpress.com/blogs/xamarin/archive/2023/01/16/cross-platform-products-2023-roadmap.aspx"),
                new BlogPost(21, "Happy New Year! What’s In Store for 2023", "Julian", new DateTime(2023,1,6), "2023_new_year","https://community.devexpress.com/blogs/news/archive/2023/01/06/happy-new-year-what_2700_s-in-store-for-2023.aspx"),
                new BlogPost(22, "DevExpress WinUI 3 Controls (v22.2) — How to Get Started", "Alex", new DateTime(2023,1,5), "winui_3_controls","https://community.devexpress.com/blogs/winui/archive/2023/01/05/devexpress-winui-3-controls-v22-2-how-to-get-started.aspx")
            };
        }
    }
}
