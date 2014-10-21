KSP Flight Computer

Features
- VPL to program vessel behavior
- Open design allowing easy addition of custom nodes
- Automatic type system. The user does not have to worry about float/double/int/string.
- Fully serializable flight programs.
- XML based category & node description
- Automatic reflection based node type registration
- Very simple node syntax

Planned features
- Action groups
- Savable programs
- Subroutines
- Variables




How to add a custom node
- Create a class library.
- Add the usual UnityEngine and Assembly-CSharp references
- Add a reference to KSPComputer.dll
- Inherit from one of the following classes (Namespace: KSPComputer.Nodes):
  - Node: Data node, no execution. Provides data in OnUpdateOutputData()
  - RootNode: A node without an incoming execution connector. Base class for "Event-Type" nodes. Needs to call "Execute()" on itself
  - ExecutableNode: A node which will be executed via an incoming connector and has at least one outgoing connector. Create executable nodes via In/Out<Connector.Exec>("Name", true/false); (true/false -> for exec connectors, multiple outputs is reverted)
- When done, create an XML file, named after your node, including the namespace. E.g. "MyNodeNamespace.NodeMyNode.xml"
- Place the .dll in the Plugins folder and the node xml in one of the category folders (or create your own category)
- Your node MUST be [Serializable]
- Done

Node syntax overview:
- If you want to add connectors (or other initialization code), override OnCreate() and add connectors via In<type>("name") or Out<type>("name"). Currently all math nodes use double as type, so you best stick to that. Other useful types are bool, vector3 and quaternion. More may be added later.
- If your node is a data node (inherits from Node), override OnUpdateOutputData() and pass data out via Out("name", data); The data should be the same type as the type defined in OnCreate(). To get input data, use In("name").AsSomeType(). No need to check for null here, a default value will always be provided.
- If your node can be executed, OnUpdateOutputData will never be called, instead use OnExecute(). To call an execution node, use ExecuteNext() or ExecuteNext("connectorName");
- To access game data, use "Program." (for example Program.Vessel, Program.VesselInfo...)

