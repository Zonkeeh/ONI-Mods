// Decompiled with JetBrains decompiler
// Type: BatchAnimCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class BatchAnimCamera : MonoBehaviour
{
  private static readonly float pan_speed = 5f;
  private static readonly float zoom_speed = 5f;
  public static Bounds bounds = new Bounds(new Vector3(0.0f, 0.0f, -50f), new Vector3(0.0f, 0.0f, 50f));
  private float zoom_min = 1f;
  private float zoom_max = 100f;
  private Camera cam;
  private bool do_pan;
  private Vector3 last_pan;

  private void Awake()
  {
    this.cam = this.GetComponent<Camera>();
  }

  private void Update()
  {
    if (Input.GetKey(KeyCode.RightArrow))
      this.transform.SetPosition(this.transform.GetPosition() + Vector3.right * BatchAnimCamera.pan_speed * Time.deltaTime);
    if (Input.GetKey(KeyCode.LeftArrow))
      this.transform.SetPosition(this.transform.GetPosition() + Vector3.left * BatchAnimCamera.pan_speed * Time.deltaTime);
    if (Input.GetKey(KeyCode.UpArrow))
      this.transform.SetPosition(this.transform.GetPosition() + Vector3.up * BatchAnimCamera.pan_speed * Time.deltaTime);
    if (Input.GetKey(KeyCode.DownArrow))
      this.transform.SetPosition(this.transform.GetPosition() + Vector3.down * BatchAnimCamera.pan_speed * Time.deltaTime);
    this.ClampToBounds();
    if (Input.GetKey(KeyCode.LeftShift))
    {
      if (Input.GetMouseButtonDown(0))
      {
        this.do_pan = true;
        this.last_pan = KInputManager.GetMousePos();
      }
      else if (Input.GetMouseButton(0) && this.do_pan)
      {
        Vector3 viewportPoint = this.cam.ScreenToViewportPoint(this.last_pan - KInputManager.GetMousePos());
        this.transform.Translate(new Vector3(viewportPoint.x * BatchAnimCamera.pan_speed, viewportPoint.y * BatchAnimCamera.pan_speed, 0.0f), Space.World);
        this.ClampToBounds();
        this.last_pan = KInputManager.GetMousePos();
      }
    }
    if (Input.GetMouseButtonUp(0))
      this.do_pan = false;
    float axis = Input.GetAxis("Mouse ScrollWheel");
    if ((double) axis == 0.0)
      return;
    this.cam.fieldOfView = Mathf.Clamp(this.cam.fieldOfView - axis * BatchAnimCamera.zoom_speed, this.zoom_min, this.zoom_max);
  }

  private void ClampToBounds()
  {
    Vector3 position = this.transform.GetPosition();
    position.x = Mathf.Clamp(this.transform.GetPosition().x, BatchAnimCamera.bounds.min.x, BatchAnimCamera.bounds.max.x);
    position.y = Mathf.Clamp(this.transform.GetPosition().y, BatchAnimCamera.bounds.min.y, BatchAnimCamera.bounds.max.y);
    position.z = Mathf.Clamp(this.transform.GetPosition().z, BatchAnimCamera.bounds.min.z, BatchAnimCamera.bounds.max.z);
    this.transform.SetPosition(position);
  }

  private void OnDrawGizmosSelected()
  {
    DebugExtension.DebugBounds(BatchAnimCamera.bounds, Color.red, 0.0f, true);
  }
}
