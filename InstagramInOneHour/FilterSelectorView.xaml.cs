using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Nokia.Graphics.Imaging;
using Nokia.InteropServices.WindowsRuntime;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;

namespace InstagramInOneHour
{
    public partial class FilterSelectorView : PhoneApplicationPage
    {
        public static ImageFilter SelectedFilter;

        // We defined a custom class called ImageFilter to manage our filters
        // In this list we can organize collect the ones we want to use
        private List<ImageFilter> filterList;

        public FilterSelectorView()
        {
            InitializeComponent();
            Loaded += FilterView_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Reset the selected filter every time the user navigates to this page
            SelectedFilter = null;
        }

        async void FilterView_Loaded(object sender, RoutedEventArgs e)
        {
            // To edit a picture with the Nokia Imaging SDK we need a FilterEffect
            // Such a filter session is based on an image and one or more filters
            // Here we create a new FilterEffect that is based on our on the MainPage selected image
            FilterEffect effect = new FilterEffect(new BitmapImageSource(MainPage.ImageToFilter.AsBitmap()));


            // Add the filter we want to offer to our users to the list
            // You can find an overview of mor filters here: http://developer.nokia.com/Resources/Library/Lumia/#!nokia-imaging-sdk.html
            filterList = new List<ImageFilter>();
            filterList.Add(new ImageFilter("Cartoon", new CartoonFilter(true)));
            filterList.Add(new ImageFilter("Antique", new AntiqueFilter()));
            filterList.Add(new ImageFilter("Color Boost", new ColorBoostFilter(2)));
            filterList.Add(new ImageFilter("Gray Scale", new GrayscaleFilter()));
            filterList.Add(new ImageFilter("Negative", new NegativeFilter()));
            filterList.Add(new ImageFilter("Sktech", new SketchFilter(SketchMode.Color)));
            filterList.Add(new ImageFilter("Mirror", new MirrorFilter()));

            // Here we add a new PivotItem for every filter we want to use
            // So the user can flip through all offered filters in the PivotControl of this page
            foreach (ImageFilter imageFilter in filterList)
            {
                // Create a new Image that we can add to each PivotItem later as a preview of the filter
                Image pivotItemImage = new Image();
                pivotItemImage.Width = 400;
                pivotItemImage.Height = 400;

                // Create the PivotItem that we want to add and set its content to the preview image we created above
                PivotItem pivotItem = new PivotItem();
                pivotItem.Header = imageFilter.Name;
                pivotItem.Content = pivotItemImage;

                // Now we add the created PivotItem to the PivotControl on this page
                FilterPivot.Items.Add(pivotItem);

                // Add the current filter
                effect.Filters = new IFilter[] { imageFilter.Filter };

                // Last we need to render the preview image
                WriteableBitmap temporaryImage = new WriteableBitmap(MainPage.ImageToFilter);
                WriteableBitmapRenderer renderer = new WriteableBitmapRenderer(effect, temporaryImage);
                await renderer.RenderAsync();
              
                pivotItemImage.Source = temporaryImage;
            }
        }

        private void ApplyFilterButton_Click(object sender, EventArgs e)
        {
            // When the user selects a filter from a previe image we get the corresponding element out of our filter list
            // and set it as the selected filter. Then we navigate back to the MainPage where we use the selected filter
            // on the current image
            SelectedFilter = filterList.ElementAt(FilterPivot.SelectedIndex);
            NavigationService.GoBack();
        }
    }
}