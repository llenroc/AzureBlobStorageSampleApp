﻿using System;

using Xamarin.Forms;

using AzureBlobStorageSampleApp.Mobile.Shared;
using EntryCustomReturn.Forms.Plugin.Abstractions;

namespace AzureBlobStorageSampleApp
{
    public class AddPhotoPage : BaseContentPage<AddPhotoViewModel>
    {
        #region Constant Fields
        readonly ToolbarItem _saveToobarItem, _cancelToolbarItem;
        readonly CustomReturnEntry _photoTitleEntry;
        readonly Image _photoImage;
        readonly Button _takePhotoButton;
        #endregion

        #region Constructors
        public AddPhotoPage()
        {
            _photoTitleEntry = new CustomReturnEntry
            {
                Placeholder = "Title",
                BackgroundColor = Color.White,
                TextColor = ColorConstants.TextColor,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                ReturnType = ReturnType.Go
            };
            _photoTitleEntry.SetBinding(Entry.TextProperty, nameof(ViewModel.PhotoTitle));
            _photoTitleEntry.SetBinding(CustomReturnEntry.ReturnCommandProperty, nameof(ViewModel.TakePhotoCommand));

            _takePhotoButton = new Button { Text = "Take Photo" };
            _takePhotoButton.SetBinding(Button.CommandProperty, nameof(ViewModel.TakePhotoCommand));

            _photoImage = new Image();
            _photoImage.SetBinding(Image.SourceProperty, nameof(ViewModel.PhotoImageSource));

            _saveToobarItem = new ToolbarItem
            {
                Text = "Save",
                Priority = 0,
                AutomationId = AutomationIdConstants.SaveButton,
            };
            _saveToobarItem.SetBinding(MenuItem.CommandProperty, nameof(ViewModel.SavePhotoCommand));
            ToolbarItems.Add(_saveToobarItem);

            _cancelToolbarItem = new ToolbarItem
            {
                Text = "Cancel",
                Priority = 1,
                AutomationId = AutomationIdConstants.CancelButton
            };
            ToolbarItems.Add(_cancelToolbarItem);

            this.SetBinding(TitleProperty, nameof(ViewModel.PhotoTitle));

            Padding = new Thickness(20);

            Content = new StackLayout
            {
                Spacing = 20,

                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,

                Children = {
                    _photoImage,
                    _photoTitleEntry,
                    _takePhotoButton,
                }
            };
        }
        #endregion

        #region Methods
        protected override void SubscribeEventHandlers()
        {
            ViewModel.NoCameraFound += HandleNoCameraFound;
            _cancelToolbarItem.Clicked += HandleCancelToolbarItemClicked;
            ViewModel.SavePhotoCompleted += HandleSavePhotoCompleted;
            ViewModel.SavePhotoFailed += HandleSavePhotoFailed;
        }

        protected override void UnsubscribeEventHandlers()
        {
            ViewModel.NoCameraFound -= HandleNoCameraFound;
            _cancelToolbarItem.Clicked -= HandleCancelToolbarItemClicked;
            ViewModel.SavePhotoCompleted -= HandleSavePhotoCompleted;
            ViewModel.SavePhotoFailed -= HandleSavePhotoFailed;
        }

        void HandleSavePhotoCompleted(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Photo Saved", string.Empty, "OK");
                ClosePage();
            });
        }

        void HandleSavePhotoFailed(object sender, string errorMessage) =>
            DisplayErrorMessage(errorMessage);

        void HandleNoCameraFound(object sender, EventArgs e) =>
            DisplayErrorMessage("No Camera Found");

        void HandleCancelToolbarItemClicked(object sender, EventArgs e) =>
            ClosePage();

        void DisplayErrorMessage(string message) =>
            Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Error", message, "Ok"));

        void ClosePage() =>
            Device.BeginInvokeOnMainThread(async () => await Navigation.PopModalAsync());
        #endregion
    }
}
