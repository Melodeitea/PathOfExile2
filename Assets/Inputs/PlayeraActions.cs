//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/Inputs/PlayeraActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayeraActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayeraActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayeraActions"",
    ""maps"": [
        {
            ""name"": ""PlayersBaseActions"",
            ""id"": ""52c743b1-0fa4-4dde-b312-e845879f53e9"",
            ""actions"": [
                {
                    ""name"": ""MoveTo"",
                    ""type"": ""Button"",
                    ""id"": ""31617bbd-368c-4c4b-bf79-a7547617ecdf"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Spell_1"",
                    ""type"": ""Button"",
                    ""id"": ""8014e8ec-3ddb-47df-8444-f1a6f42c9692"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7ec7f1ce-2299-47dd-a728-d978d431e3cf"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveTo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ea19f8b-d596-4104-91cf-a2b696c49bdb"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Spell_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayersBaseActions
        m_PlayersBaseActions = asset.FindActionMap("PlayersBaseActions", throwIfNotFound: true);
        m_PlayersBaseActions_MoveTo = m_PlayersBaseActions.FindAction("MoveTo", throwIfNotFound: true);
        m_PlayersBaseActions_Spell_1 = m_PlayersBaseActions.FindAction("Spell_1", throwIfNotFound: true);
    }

    ~@PlayeraActions()
    {
        UnityEngine.Debug.Assert(!m_PlayersBaseActions.enabled, "This will cause a leak and performance issues, PlayeraActions.PlayersBaseActions.Disable() has not been called.");
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayersBaseActions
    private readonly InputActionMap m_PlayersBaseActions;
    private List<IPlayersBaseActionsActions> m_PlayersBaseActionsActionsCallbackInterfaces = new List<IPlayersBaseActionsActions>();
    private readonly InputAction m_PlayersBaseActions_MoveTo;
    private readonly InputAction m_PlayersBaseActions_Spell_1;
    public struct PlayersBaseActionsActions
    {
        private @PlayeraActions m_Wrapper;
        public PlayersBaseActionsActions(@PlayeraActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveTo => m_Wrapper.m_PlayersBaseActions_MoveTo;
        public InputAction @Spell_1 => m_Wrapper.m_PlayersBaseActions_Spell_1;
        public InputActionMap Get() { return m_Wrapper.m_PlayersBaseActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayersBaseActionsActions set) { return set.Get(); }
        public void AddCallbacks(IPlayersBaseActionsActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayersBaseActionsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayersBaseActionsActionsCallbackInterfaces.Add(instance);
            @MoveTo.started += instance.OnMoveTo;
            @MoveTo.performed += instance.OnMoveTo;
            @MoveTo.canceled += instance.OnMoveTo;
            @Spell_1.started += instance.OnSpell_1;
            @Spell_1.performed += instance.OnSpell_1;
            @Spell_1.canceled += instance.OnSpell_1;
        }

        private void UnregisterCallbacks(IPlayersBaseActionsActions instance)
        {
            @MoveTo.started -= instance.OnMoveTo;
            @MoveTo.performed -= instance.OnMoveTo;
            @MoveTo.canceled -= instance.OnMoveTo;
            @Spell_1.started -= instance.OnSpell_1;
            @Spell_1.performed -= instance.OnSpell_1;
            @Spell_1.canceled -= instance.OnSpell_1;
        }

        public void RemoveCallbacks(IPlayersBaseActionsActions instance)
        {
            if (m_Wrapper.m_PlayersBaseActionsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayersBaseActionsActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayersBaseActionsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayersBaseActionsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayersBaseActionsActions @PlayersBaseActions => new PlayersBaseActionsActions(this);
    public interface IPlayersBaseActionsActions
    {
        void OnMoveTo(InputAction.CallbackContext context);
        void OnSpell_1(InputAction.CallbackContext context);
    }
}
