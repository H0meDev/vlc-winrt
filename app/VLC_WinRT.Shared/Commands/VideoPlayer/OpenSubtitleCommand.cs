﻿/**********************************************************************
 * VLC for WinRT
 **********************************************************************
 * Copyright © 2013-2014 VideoLAN and Authors
 *
 * Licensed under GPLv2+ and MPLv2
 * Refer to COPYING file of the official project for license
 **********************************************************************/

using System;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using VLC_WinRT.Model;
using Windows.Storage;
using Windows.Storage.AccessCache;
using VLC_WinRT.ViewModels;
using VLC_WinRT.Helpers;
using VLC_WinRT.Utils;

namespace VLC_WinRT.Commands.VideoPlayer
{
    public class OpenSubtitleCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            if (parameter is StorageFile)
            {
                OpenSubtitleFile((StorageFile)parameter);
            }
            else
            {
                String error;
                try
                {
                    App.OpenFilePickerReason = OpenFilePickerReason.OnOpeningSubtitle;
                    var picker = new FileOpenPicker
                    {
                        ViewMode = PickerViewMode.List,
                        SuggestedStartLocation = PickerLocationId.VideosLibrary
                    };
                    picker.FileTypeFilter.Add(".srt");
                    picker.FileTypeFilter.Add(".ass");
#if WINDOWS_PHONE_APP
                    picker.PickSingleFileAndContinue();
#else
                    StorageFile file = await picker.PickSingleFileAsync();
                    if (file != null)
                    {
                        OpenSubtitleFile(file);
                    }
                    else
                    {
                        LogHelper.Log("Cancelled Opening subtitle");
                    }
                    App.OpenFilePickerReason = OpenFilePickerReason.Null;
#endif
                    return;
                }
                catch (Exception exception)
                {
                    error = exception.ToString();
                }
                var dialog = new MessageDialog(error);
                await dialog.ShowAsync();
            }
        }

        void OpenSubtitleFile(StorageFile file)
        {
            string mrl = "winrt://" + StorageApplicationPermissions.FutureAccessList.Add(file);
            Locator.MediaPlaybackViewModel.OpenSubtitleMrl(mrl);
        }
    }
}
