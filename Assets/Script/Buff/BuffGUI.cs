using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Buff_GUI : MonoBehaviour
{
    [Header("Buff Slots")]
    public BuffSlot[] buffSlots;

    private int currentIndex = 0;
    private bool isActive = false;

    private Action<string> onBuffSelected;

    private PlayerInputAction input;

    private void Awake()
    {
        input = new PlayerInputAction();

        input.Input.Navigate.performed += OnNavigate;

        input.Input.Confirm.performed += OnConfirm;

        HideAll();

    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void ShowBuffs(string[] buffIDs, Action<string> callback)
    {
        onBuffSelected = callback;
        isActive = true;

        currentIndex = 0;

        for (int i = 0; i < buffSlots.Length; i++)
        {
            if (i < buffIDs.Length &&
                BuffLibrary.AllBuffs.TryGetValue(buffIDs[i], out Buff buff))
            {
                buffSlots[i].gameObject.SetActive(true);
                buffSlots[i].Setup(buff, OnSlotSelected);
            }
            else
            {
                buffSlots[i].gameObject.SetActive(false);
            }
        }

        UpdateHighlight();
    }

    public void HideAll()
    {
        isActive = false;

        foreach (var slot in buffSlots)
        {
            slot.Highlight(false);
            slot.gameObject.SetActive(false);
        }
    }

    private void OnNavigate(InputAction.CallbackContext ctx)
    {
        if (!isActive) return;
        if (!HasAnyActiveSlot()) return; 
        Vector2 dir = ctx.ReadValue<Vector2>();

        if (dir.sqrMagnitude < 0.5f) return;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0.5f) Move(1);
            else if (dir.x < -0.5f) Move(-1);
        }
        else
        {
            if (dir.y > 0.5f) Move(-1);
            else if (dir.y < -0.5f) Move(1);
        }
    }


    private void OnConfirm(InputAction.CallbackContext ctx)
    {
        if (!isActive) return;

        buffSlots[currentIndex].Select();
    }

    private void Move(int delta)
    {
        int next = currentIndex;

        do
        {
            next += delta;

            if (next < 0)
                next = buffSlots.Length - 1;
            else if (next >= buffSlots.Length)
                next = 0;

        } while (!buffSlots[next].gameObject.activeSelf);

        currentIndex = next;
        UpdateHighlight();
    }

    private void UpdateHighlight()
    {
        for (int i = 0; i < buffSlots.Length; i++)
        {
            buffSlots[i].Highlight(i == currentIndex);
        }
    }

    private void OnSlotSelected(string buffID)
    {
        onBuffSelected?.Invoke(buffID);
        HideAll();
    }

    private bool HasAnyActiveSlot()
    {
        foreach (var slot in buffSlots)
        {
            if (slot.gameObject.activeSelf)
                return true;
        }
        return false;
    }

}
