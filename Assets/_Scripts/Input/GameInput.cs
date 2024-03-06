// GENERATED AUTOMATICALLY FROM 'Assets/GameInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInput"",
    ""maps"": [
        {
            ""name"": ""Gamepay"",
            ""id"": ""0a25223b-c0d5-4969-83a4-e2d05c42f625"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""d94c58f2-9862-40a3-b438-b9a28e85eb4e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""26290039-6d2d-4c89-b5fa-033f32cc3e7c"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""519fa2fe-e968-427e-82a4-61c2c9038e5e"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""515bde91-1edd-4e9b-89de-41aef73592ee"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""9d030980-43b7-41be-865f-7990723dc70e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""a0844b77-d735-495b-b88d-4766eabb9965"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3eda2652-9c25-436b-bed0-bc64740906ac"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""142dfd46-ccb4-4bfb-ad35-1c0e6d0dbcd2"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""83f8a87d-ef48-4552-8206-d398e72a5031"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e5ab8c9b-8ef6-43e5-8cda-d745987788db"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""62a5d12d-a4c6-438b-aa4a-144c043bd14d"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gamepay
        m_Gamepay = asset.FindActionMap("Gamepay", throwIfNotFound: true);
        m_Gamepay_Newaction = m_Gamepay.FindAction("New action", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Move = m_UI.FindAction("Move", throwIfNotFound: true);
        m_UI_Jump = m_UI.FindAction("Jump", throwIfNotFound: true);
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

    // Gamepay
    private readonly InputActionMap m_Gamepay;
    private IGamepayActions m_GamepayActionsCallbackInterface;
    private readonly InputAction m_Gamepay_Newaction;
    public struct GamepayActions
    {
        private @GameInput m_Wrapper;
        public GamepayActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_Gamepay_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_Gamepay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GamepayActions set) { return set.Get(); }
        public void SetCallbacks(IGamepayActions instance)
        {
            if (m_Wrapper.m_GamepayActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_GamepayActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_GamepayActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_GamepayActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_GamepayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
            }
        }
    }
    public GamepayActions @Gamepay => new GamepayActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Move;
    private readonly InputAction m_UI_Jump;
    public struct UIActions
    {
        private @GameInput m_Wrapper;
        public UIActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_UI_Move;
        public InputAction @Jump => m_Wrapper.m_UI_Jump;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_UIActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnJump;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    public interface IGamepayActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
    }
}
