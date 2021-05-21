// GENERATED AUTOMATICALLY FROM 'Assets/Input Actions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Input Actions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""9c611ce3-4ac7-4517-933a-07c7c113d738"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""24ae5c2f-ec11-45bf-bf29-792936f4fe0f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""InvertVector2(invertX=false,invertY=false)"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""827ae8c5-b9c9-4eff-ae3e-e788c08a27d1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Boost"",
                    ""type"": ""Button"",
                    ""id"": ""9827d522-3c05-4a73-99c3-a820bb166ce4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""b200b0d3-0bab-4363-9c16-841f13985b5e"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""da7baadf-67ec-45ae-82d4-c5ae2ca6a605"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4dbef587-e65f-4ef1-bf82-fd9817ed648d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""511f1670-b577-4d38-a7bf-5bb01f2424cd"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8f13286d-daba-4029-9127-cda7206f7114"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c1698247-db18-431e-b4f2-570b176c0bab"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""211d004d-8fb2-44a0-b5d4-c3a5cf9e0333"",
                    ""path"": ""<Joystick>/stick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""22eeacb0-3e48-4549-ba7f-1188945d5aa6"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b67663e4-2fc9-493a-ad3a-fcc7f06b6e21"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9a54ef17-c1b6-4378-a759-eccb714074f9"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/button3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b321daf1-2602-4e28-9bec-9701ee404961"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""065a27fb-3ac8-48b1-9d26-9e925442f67d"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d4688da-a044-4da1-85df-edf154367740"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/button8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Camera"",
            ""id"": ""84e184e6-cd54-44a9-82c7-1fda8c9431be"",
            ""actions"": [
                {
                    ""name"": ""HorizontalRotate"",
                    ""type"": ""Value"",
                    ""id"": ""13b41ae0-6b71-4dd1-a864-1d2715703cca"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": ""Invert"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""VerticalRotate"",
                    ""type"": ""Value"",
                    ""id"": ""0d182bfa-c72b-4099-8ae4-6c0204baefd0"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": ""Invert"",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""836ee529-45b6-4503-9e13-4457e0cc6766"",
                    ""path"": ""<Gamepad>/rightStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""VerticalRotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f198f60d-9f42-4721-bc25-2748832627a6"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=0.3)"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""VerticalRotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""adfbe921-de32-4297-a20f-79812f6c59aa"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/rz"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""VerticalRotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""abbd9502-6411-4d82-b2d2-b6ae2c0d6bb4"",
                    ""path"": ""<Gamepad>/rightStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""HorizontalRotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bfd55753-26aa-4edb-bf9f-4f5ac632cab1"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=0.3)"",
                    ""groups"": ""Keyboard"",
                    ""action"": ""HorizontalRotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93d3570e-eb6d-48c0-90b8-62ee3def3a89"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""HorizontalRotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""GamePad"",
            ""bindingGroup"": ""GamePad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Boost = m_Player.FindAction("Boost", throwIfNotFound: true);
        // Camera
        m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        m_Camera_HorizontalRotate = m_Camera.FindAction("HorizontalRotate", throwIfNotFound: true);
        m_Camera_VerticalRotate = m_Camera.FindAction("VerticalRotate", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Boost;
    public struct PlayerActions
    {
        private @InputActions m_Wrapper;
        public PlayerActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Boost => m_Wrapper.m_Player_Boost;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Boost.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBoost;
                @Boost.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBoost;
                @Boost.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBoost;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Boost.started += instance.OnBoost;
                @Boost.performed += instance.OnBoost;
                @Boost.canceled += instance.OnBoost;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Camera
    private readonly InputActionMap m_Camera;
    private ICameraActions m_CameraActionsCallbackInterface;
    private readonly InputAction m_Camera_HorizontalRotate;
    private readonly InputAction m_Camera_VerticalRotate;
    public struct CameraActions
    {
        private @InputActions m_Wrapper;
        public CameraActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @HorizontalRotate => m_Wrapper.m_Camera_HorizontalRotate;
        public InputAction @VerticalRotate => m_Wrapper.m_Camera_VerticalRotate;
        public InputActionMap Get() { return m_Wrapper.m_Camera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
        public void SetCallbacks(ICameraActions instance)
        {
            if (m_Wrapper.m_CameraActionsCallbackInterface != null)
            {
                @HorizontalRotate.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnHorizontalRotate;
                @HorizontalRotate.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnHorizontalRotate;
                @HorizontalRotate.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnHorizontalRotate;
                @VerticalRotate.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnVerticalRotate;
                @VerticalRotate.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnVerticalRotate;
                @VerticalRotate.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnVerticalRotate;
            }
            m_Wrapper.m_CameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HorizontalRotate.started += instance.OnHorizontalRotate;
                @HorizontalRotate.performed += instance.OnHorizontalRotate;
                @HorizontalRotate.canceled += instance.OnHorizontalRotate;
                @VerticalRotate.started += instance.OnVerticalRotate;
                @VerticalRotate.performed += instance.OnVerticalRotate;
                @VerticalRotate.canceled += instance.OnVerticalRotate;
            }
        }
    }
    public CameraActions @Camera => new CameraActions(this);
    private int m_GamePadSchemeIndex = -1;
    public InputControlScheme GamePadScheme
    {
        get
        {
            if (m_GamePadSchemeIndex == -1) m_GamePadSchemeIndex = asset.FindControlSchemeIndex("GamePad");
            return asset.controlSchemes[m_GamePadSchemeIndex];
        }
    }
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnBoost(InputAction.CallbackContext context);
    }
    public interface ICameraActions
    {
        void OnHorizontalRotate(InputAction.CallbackContext context);
        void OnVerticalRotate(InputAction.CallbackContext context);
    }
}
