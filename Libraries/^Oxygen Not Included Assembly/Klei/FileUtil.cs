// Decompiled with JetBrains decompiler
// Type: Klei.FileUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Klei
{
  public static class FileUtil
  {
    private const FileUtil.Test TEST = FileUtil.Test.NoTesting;
    private const int DEFAULT_RETRY_COUNT = 0;
    private const int RETRY_MILLISECONDS = 100;

    public static void ErrorDialog(string msg)
    {
      Debug.Log((object) msg);
      ConfirmDialogScreen component = Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, !((UnityEngine.Object) FrontEndManager.Instance == (UnityEngine.Object) null) ? FrontEndManager.Instance.gameObject : GameScreenManager.Instance.ssOverlayCanvas, true).GetComponent<ConfirmDialogScreen>();
      component.PopupConfirmDialog(msg, (System.Action) null, (System.Action) null, (string) null, (System.Action) null, (string) null, (string) null, (string) null, (Sprite) null, true);
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) component.gameObject);
    }

    public static T DoIOFunc<T>(Func<T> io_op, int retry_count = 0)
    {
      UnauthorizedAccessException unauthorizedAccessException = (UnauthorizedAccessException) null;
      IOException ioException = (IOException) null;
      Exception exception = (Exception) null;
      for (int index = 0; index <= retry_count; ++index)
      {
        try
        {
          return io_op();
        }
        catch (UnauthorizedAccessException ex)
        {
          unauthorizedAccessException = ex;
        }
        catch (IOException ex)
        {
          ioException = ex;
        }
        catch (Exception ex)
        {
          exception = ex;
        }
        Thread.Sleep(index * 100);
      }
      if (unauthorizedAccessException != null)
        throw unauthorizedAccessException;
      if (ioException != null)
        throw ioException;
      if (exception != null)
        throw exception;
      throw new Exception("Unreachable code path in FileUtil::DoIOFunc()");
    }

    public static void DoIOAction(System.Action io_op, int retry_count = 0)
    {
      UnauthorizedAccessException unauthorizedAccessException = (UnauthorizedAccessException) null;
      IOException ioException = (IOException) null;
      Exception exception = (Exception) null;
      for (int index = 0; index <= retry_count; ++index)
      {
        try
        {
          io_op();
          return;
        }
        catch (UnauthorizedAccessException ex)
        {
          unauthorizedAccessException = ex;
        }
        catch (IOException ex)
        {
          ioException = ex;
        }
        catch (Exception ex)
        {
          exception = ex;
        }
        Thread.Sleep(index * 100);
      }
      if (unauthorizedAccessException != null)
        throw unauthorizedAccessException;
      if (ioException != null)
        throw ioException;
      if (exception != null)
        throw exception;
      throw new Exception("Unreachable code path in FileUtil::DoIOAction()");
    }

    public static T DoIODialog<T>(
      Func<T> io_op,
      string io_subject,
      T fail_result,
      int retry_count = 0)
    {
      try
      {
        return FileUtil.DoIOFunc<T>(io_op, retry_count);
      }
      catch (UnauthorizedAccessException ex)
      {
        FileUtil.ErrorDialog(string.Format((string) UI.FRONTEND.SUPPORTWARNINGS.IO_UNAUTHORIZED, (object) io_subject));
      }
      catch (IOException ex)
      {
        FileUtil.ErrorDialog(string.Format((string) UI.FRONTEND.SUPPORTWARNINGS.IO_SUFFICIENT_SPACE, (object) io_subject));
      }
      catch
      {
        throw;
      }
      return fail_result;
    }

    public static FileStream Create(string filename, int retry_count = 0)
    {
      return FileUtil.DoIODialog<FileStream>((Func<FileStream>) (() => File.Create(filename)), filename, (FileStream) null, retry_count);
    }

    public static bool CreateDirectory(string path, int retry_count = 0)
    {
      return FileUtil.DoIODialog<bool>((Func<bool>) (() =>
      {
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        return true;
      }), path, false, retry_count);
    }

    public static bool DeleteDirectory(string path, int retry_count = 0)
    {
      return FileUtil.DoIODialog<bool>((Func<bool>) (() =>
      {
        if (!Directory.Exists(path))
          return true;
        Directory.Delete(path, true);
        return true;
      }), path, false, retry_count);
    }

    public static bool FileExists(string filename, int retry_count = 0)
    {
      return FileUtil.DoIODialog<bool>((Func<bool>) (() => File.Exists(filename)), filename, false, retry_count);
    }

    private enum Test
    {
      NoTesting,
      RetryOnce,
    }
  }
}
