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
                    ""processors"": ""StickDeadzone(max=1)"",
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
                },
                {
                    ""name"": ""VertBoost"",
                    ""type"": ""Button"",
                    ""id"": ""16f7d561-e9a3-46a8-a38b-b8100dd8eef6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dive"",
                    ""type"": ""Button"",
                    ""id"": ""1ab046c3-dd5f-4518-8a99-a8c18c74d186"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GroundPound"",
                    ""type"": ""Button"",
                    ""id"": ""f04f2731-960c-4145-80a9-ec9ce4d4ec92"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Glide"",
                    ""type"": ""Button"",
                    ""id"": ""9352f2b2-3ea4-4746-a1fa-73abf04be3bb"",
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
                    ""id"": ""0d4688da-a044-4da1-85df-edf154367740"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/button8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3c81bc6-4fcf-4f3f-acf2-9ea485476562"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""VertBoost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6609c790-175a-464f-b2b7-decc2ac6f3cd"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/button7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""VertBoost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e2114d78-f06b-47ae-9078-0f69d8279aff"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VertBoost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""556bf8ae-432c-4785-9ca5-a8da11cb1988"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/button3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""VertBoost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""16627e8e-72c9-4b9d-837c-3c93d769e8df"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Dive"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d00335ae-e0f7-45a2-8a8c-de5f36bdf7bf"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Dive"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""279b09dc-bbdc-4940-93b6-5242e0cf1212"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/button2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Dive"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af91bb02-0f15-4ce8-a13a-d4622ce8209c"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3cce94b2-2eaf-4978-937d-16cd6d1a78e1"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""GroundPound"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f3d11900-6e60-4ca2-9ca0-44e4724ef9c9"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""GroundPound"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b3e30e7d-9d16-4b99-8a0d-53aad700e982"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""GroundPound"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""77fb4ea4-8ff4-4a91-a506-c150ee2cc2a5"",
                    ""path"": ""<Mouse>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Glide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""00f709b6-678f-4ef9-a39c-bb55cbbd1e76"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Glide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6bb6895c-e9f7-49d6-9c6f-713713562eb7"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/button4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Glide"",
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
                    ""processors"": ""Invert,AxisDeadzone(max=1)"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""VerticalRotate"",
                    ""type"": ""Value"",
                    ""id"": ""0d182bfa-c72b-4099-8ae4-6c0204baefd0"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": ""Invert,AxisDeadzone(max=1)"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AutoAdjust"",
                    ""type"": ""Button"",
                    ""id"": ""e2c55936-ee8b-47db-ad02-66ffec06334b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": ""Invert,AxisDeadzone(max=1)"",
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
                    ""id"": ""93d3570e-eb6d-48c0-90b8-62ee3def3a89"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""HorizontalRotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3947540-f52f-4082-a766-5dec348e3b2d"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""AutoAdjust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""817fa2e9-a660-4ab2-8a99-1257785032c0"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AutoAdjust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d4e2eff9-040e-43a4-a6d7-26c9827d7abe"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AutoAdjust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f23fea8a-5033-4352-b97f-57254a4ec39d"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/button5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""AutoAdjust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""19091afe-d70d-4185-9026-a21d166164da"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/button6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""AutoAdjust"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Debug"",
            ""id"": ""31899676-c826-4893-af5b-a55b80294e3f"",
            ""actions"": [
                {
                    ""name"": ""Freeze"",
                    ""type"": ""Button"",
                    ""id"": ""3cb7b2f8-0e23-449e-8229-9fd53f912a29"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""835d3680-1351-4c6b-a9a9-6c9ccdd6cead"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Freeze"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Checkpoint"",
            ""id"": ""7b0a17c1-61b1-44af-b970-358bda0d9eac"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""05b4785d-a231-4542-a363-be9432f8ba06"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Respawn"",
                    ""type"": ""Button"",
                    ""id"": ""e5cc6880-a11b-4cbc-82be-b5b49164e129"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""56047875-7c73-4e11-8a82-cac8502deae1"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""76f52b3a-9eb1-4fd0-a964-2e456e77799d"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Respawn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2861122b-fc42-45d8-b3d6-1541223500b3"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/hat/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Respawn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""64b9d252-d6c3-448c-9d11-e57b393660c5"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""243f9154-54b7-42e4-957d-77779f5f5f32"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""08b7d9e4-0023-4f2b-97f2-0c9fe3408d03"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""84993e81-1e0d-4bd8-9c03-e5c41616ee49"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""4de7ad40-571f-49f4-852e-26b234678ab2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""bdb05aea-0abb-4b74-b55d-784154c155f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d8047329-7fcd-43f4-95e0-764c117d1b8f"",
                    ""path"": ""<Joystick>/stick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""804f8cf6-fb31-4fcb-9450-9edd4db78310"",
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
                    ""id"": ""bd064fad-d87c-4bf9-89c3-175744153a3c"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b8399ffe-d224-4839-a8e3-70cb3dccd282"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2991e576-482a-4992-a98a-96f2acca57b6"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0f0c250f-e96d-4d83-bd2f-75d925fe24ae"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""10cf04f2-e2e5-4182-8a4a-0085c0eeb837"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/button3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""439838ef-d09c-4ee9-beaa-e1919984887e"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38548651-feb4-48b5-b80e-23a3c10d5835"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/button10"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a18652a7-3bcf-47c3-ab0d-c503640c08f0"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b8997d61-3818-43c9-ad9f-838fd6637b42"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/button2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e5bc17e-4130-488f-ba90-27dea410d42d"",
                    ""path"": ""<HID::Core (Plus) Wired Controller>/rz"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Zoom"",
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
        m_Player_VertBoost = m_Player.FindAction("VertBoost", throwIfNotFound: true);
        m_Player_Dive = m_Player.FindAction("Dive", throwIfNotFound: true);
        m_Player_GroundPound = m_Player.FindAction("GroundPound", throwIfNotFound: true);
        m_Player_Glide = m_Player.FindAction("Glide", throwIfNotFound: true);
        // Camera
        m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        m_Camera_HorizontalRotate = m_Camera.FindAction("HorizontalRotate", throwIfNotFound: true);
        m_Camera_VerticalRotate = m_Camera.FindAction("VerticalRotate", throwIfNotFound: true);
        m_Camera_AutoAdjust = m_Camera.FindAction("AutoAdjust", throwIfNotFound: true);
        // Debug
        m_Debug = asset.FindActionMap("Debug", throwIfNotFound: true);
        m_Debug_Freeze = m_Debug.FindAction("Freeze", throwIfNotFound: true);
        // Checkpoint
        m_Checkpoint = asset.FindActionMap("Checkpoint", throwIfNotFound: true);
        m_Checkpoint_Newaction = m_Checkpoint.FindAction("New action", throwIfNotFound: true);
        m_Checkpoint_Respawn = m_Checkpoint.FindAction("Respawn", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Move = m_UI.FindAction("Move", throwIfNotFound: true);
        m_UI_Zoom = m_UI.FindAction("Zoom", throwIfNotFound: true);
        m_UI_Select = m_UI.FindAction("Select", throwIfNotFound: true);
        m_UI_Pause = m_UI.FindAction("Pause", throwIfNotFound: true);
        m_UI_Back = m_UI.FindAction("Back", throwIfNotFound: true);
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
    private readonly InputAction m_Player_VertBoost;
    private readonly InputAction m_Player_Dive;
    private readonly InputAction m_Player_GroundPound;
    private readonly InputAction m_Player_Glide;
    public struct PlayerActions
    {
        private @InputActions m_Wrapper;
        public PlayerActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Boost => m_Wrapper.m_Player_Boost;
        public InputAction @VertBoost => m_Wrapper.m_Player_VertBoost;
        public InputAction @Dive => m_Wrapper.m_Player_Dive;
        public InputAction @GroundPound => m_Wrapper.m_Player_GroundPound;
        public InputAction @Glide => m_Wrapper.m_Player_Glide;
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
                @VertBoost.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnVertBoost;
                @VertBoost.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnVertBoost;
                @VertBoost.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnVertBoost;
                @Dive.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDive;
                @Dive.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDive;
                @Dive.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDive;
                @GroundPound.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGroundPound;
                @GroundPound.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGroundPound;
                @GroundPound.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGroundPound;
                @Glide.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGlide;
                @Glide.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGlide;
                @Glide.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGlide;
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
                @VertBoost.started += instance.OnVertBoost;
                @VertBoost.performed += instance.OnVertBoost;
                @VertBoost.canceled += instance.OnVertBoost;
                @Dive.started += instance.OnDive;
                @Dive.performed += instance.OnDive;
                @Dive.canceled += instance.OnDive;
                @GroundPound.started += instance.OnGroundPound;
                @GroundPound.performed += instance.OnGroundPound;
                @GroundPound.canceled += instance.OnGroundPound;
                @Glide.started += instance.OnGlide;
                @Glide.performed += instance.OnGlide;
                @Glide.canceled += instance.OnGlide;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Camera
    private readonly InputActionMap m_Camera;
    private ICameraActions m_CameraActionsCallbackInterface;
    private readonly InputAction m_Camera_HorizontalRotate;
    private readonly InputAction m_Camera_VerticalRotate;
    private readonly InputAction m_Camera_AutoAdjust;
    public struct CameraActions
    {
        private @InputActions m_Wrapper;
        public CameraActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @HorizontalRotate => m_Wrapper.m_Camera_HorizontalRotate;
        public InputAction @VerticalRotate => m_Wrapper.m_Camera_VerticalRotate;
        public InputAction @AutoAdjust => m_Wrapper.m_Camera_AutoAdjust;
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
                @AutoAdjust.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnAutoAdjust;
                @AutoAdjust.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnAutoAdjust;
                @AutoAdjust.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnAutoAdjust;
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
                @AutoAdjust.started += instance.OnAutoAdjust;
                @AutoAdjust.performed += instance.OnAutoAdjust;
                @AutoAdjust.canceled += instance.OnAutoAdjust;
            }
        }
    }
    public CameraActions @Camera => new CameraActions(this);

    // Debug
    private readonly InputActionMap m_Debug;
    private IDebugActions m_DebugActionsCallbackInterface;
    private readonly InputAction m_Debug_Freeze;
    public struct DebugActions
    {
        private @InputActions m_Wrapper;
        public DebugActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Freeze => m_Wrapper.m_Debug_Freeze;
        public InputActionMap Get() { return m_Wrapper.m_Debug; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DebugActions set) { return set.Get(); }
        public void SetCallbacks(IDebugActions instance)
        {
            if (m_Wrapper.m_DebugActionsCallbackInterface != null)
            {
                @Freeze.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnFreeze;
                @Freeze.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnFreeze;
                @Freeze.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnFreeze;
            }
            m_Wrapper.m_DebugActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Freeze.started += instance.OnFreeze;
                @Freeze.performed += instance.OnFreeze;
                @Freeze.canceled += instance.OnFreeze;
            }
        }
    }
    public DebugActions @Debug => new DebugActions(this);

    // Checkpoint
    private readonly InputActionMap m_Checkpoint;
    private ICheckpointActions m_CheckpointActionsCallbackInterface;
    private readonly InputAction m_Checkpoint_Newaction;
    private readonly InputAction m_Checkpoint_Respawn;
    public struct CheckpointActions
    {
        private @InputActions m_Wrapper;
        public CheckpointActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_Checkpoint_Newaction;
        public InputAction @Respawn => m_Wrapper.m_Checkpoint_Respawn;
        public InputActionMap Get() { return m_Wrapper.m_Checkpoint; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CheckpointActions set) { return set.Get(); }
        public void SetCallbacks(ICheckpointActions instance)
        {
            if (m_Wrapper.m_CheckpointActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_CheckpointActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_CheckpointActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_CheckpointActionsCallbackInterface.OnNewaction;
                @Respawn.started -= m_Wrapper.m_CheckpointActionsCallbackInterface.OnRespawn;
                @Respawn.performed -= m_Wrapper.m_CheckpointActionsCallbackInterface.OnRespawn;
                @Respawn.canceled -= m_Wrapper.m_CheckpointActionsCallbackInterface.OnRespawn;
            }
            m_Wrapper.m_CheckpointActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
                @Respawn.started += instance.OnRespawn;
                @Respawn.performed += instance.OnRespawn;
                @Respawn.canceled += instance.OnRespawn;
            }
        }
    }
    public CheckpointActions @Checkpoint => new CheckpointActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Move;
    private readonly InputAction m_UI_Zoom;
    private readonly InputAction m_UI_Select;
    private readonly InputAction m_UI_Pause;
    private readonly InputAction m_UI_Back;
    public struct UIActions
    {
        private @InputActions m_Wrapper;
        public UIActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_UI_Move;
        public InputAction @Zoom => m_Wrapper.m_UI_Zoom;
        public InputAction @Select => m_Wrapper.m_UI_Select;
        public InputAction @Pause => m_Wrapper.m_UI_Pause;
        public InputAction @Back => m_Wrapper.m_UI_Back;
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
                @Zoom.started -= m_Wrapper.m_UIActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnZoom;
                @Select.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSelect;
                @Pause.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Back.started -= m_Wrapper.m_UIActionsCallbackInterface.OnBack;
                @Back.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnBack;
                @Back.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnBack;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Back.started += instance.OnBack;
                @Back.performed += instance.OnBack;
                @Back.canceled += instance.OnBack;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
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
        void OnVertBoost(InputAction.CallbackContext context);
        void OnDive(InputAction.CallbackContext context);
        void OnGroundPound(InputAction.CallbackContext context);
        void OnGlide(InputAction.CallbackContext context);
    }
    public interface ICameraActions
    {
        void OnHorizontalRotate(InputAction.CallbackContext context);
        void OnVerticalRotate(InputAction.CallbackContext context);
        void OnAutoAdjust(InputAction.CallbackContext context);
    }
    public interface IDebugActions
    {
        void OnFreeze(InputAction.CallbackContext context);
    }
    public interface ICheckpointActions
    {
        void OnNewaction(InputAction.CallbackContext context);
        void OnRespawn(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnBack(InputAction.CallbackContext context);
    }
}
