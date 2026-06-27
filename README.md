# Team Fortress 2 Classified
🧩 Plugin for WindowsGSM to run a dedicated server for Team Fortress 2 Classified

## PLEASE ⭐STAR⭐ THE REPO IF YOU LIKE IT! THANKS!

### IMPORTANT POINTS, READ FIRST:
- You need to do **Portforwarding**, if you want people outside of your house to be able to connect (look down for it)
 - If you want your Server to be Listed: (default is LAN/IP-Connect ONLY!)
  - **install the SteamClient (start it once)** — no login or autostart required
  - go to the Server.cfg (WindowsGSM Click on **Browse => Server Files**, then go **\tf2\cfg** and open **server.cfg** with Editor)
    - Change `sv_lan` from `1` to `0`
- The Server can not be **connected from the same Machine**. You will need to create the server ingame (Multiplayer => create) if one Machine has to act as Server AND Client.

### WindowsGSM Installation: 
1. Download  WindowsGSM https://windowsgsm.com/ 
2. Create a Folder at a Location you wan't all Server to be Installed and Run.
3. Drag WindowsGSM.Exe into previously created folder and execute it.

### Plugin Installation:
1. Download [latest](https://github.com/Heapons/WindowsGSM.TF2_Classified/releases/latest) release
2. Either Extract then Move the folder **TF2_Classified.cs** to **WindowsGSM/plugins** 
    1. Press on the Puzzle Icon in the left bottom side and press **[RELOAD PLUGINS]** or restart WindowsGSM
3. Or Press on the Puzzle Icon in the left bottom side and press **[IMPORT PLUGIN]** and choose the downloaded .zip

### Configuration
Modify the server config file to adjust settings (Browse => Server Files): `serverfiles\tf2classified\cfg`

**WindowsGSM.cfg settings**
- `Enable Steam Networking` — add `enable_steam_networking` to the Server Parameters; controls `sv_use_steam_networking <0|1>`.
- `Custom Weapons` — add `customweapons` to the Server Parameters to append `type_customweapons` to `sv_tags`.
- `Custom Rules` — add `customrules` to the Server Parameters to append `type_customrules` to `sv_tags`.

Notes: The plugin will replace the placeholders `{{enable_steam_networking}}` and `{{sv_tags_extra}}` in `WindowsGSM.cfg` when present. If placeholders are not found, the plugin will append the `sv_use_steam_networking` line and attempt to add tag tokens to an existing `sv_tags` line.

### Official Documentation
🗃️ Didn't find any documentation yet. Please Let me know if you came accros one

### The Game
🕹️ https://store.steampowered.com/app/3545060/Team_Fortress_2_Classified/

### Dedicated server info
🖥️ https://steamdb.info/app/3557020/info/

### Port Forwarding (YOU NEED THIS, TO BE ABLE TO CONNECT FROM THE INTERNET(only for servers/pcs at home, Hosted Servers work different there, make sure you have a full IPv4 and if your provider needs explicit portforwarding):
- If You don't know How: Google: YourRouterBrand + Portforwarding
- 27015 UDP - **Default Game Port**(look in the 7th column in WindowsGSM), 
  - this one is needed to be forwarded in your Router!
- 27015 TCP - **QueryPort** default, in theory the QueryPort should not be needed as PortForward

### Connecting to a LAN Server:
- Go to Opptions
- Enable Developer Mode
- Go To Keyboard Bindings and set the Key for the Console
- go Back
- Press that key and enter
  - connect IP_OF_THE_SERVER:GamePort (if port is 27015 you can leave it out)
  - if your in the same network use the local IP of that server, else use you onlineIP
  - https://www.whatismyip.com/
  
### Not having an full IPv4 adress ( named CCNAT or DSL Light )
No game or gameserver supports ipv6 only connections. 
- You need to either buy one (most VPN services provide that option. A pal uses ovpn.net for his server, I know of nordvpn also providing that. Should both cost around 7€ cheaper half of it, if your already having an VPN)
- Or you pay a bit more for your internet and take a contract with full ipv4. (depending on your country)
- There are also tunneling methods, which require acces to a server with a full ipv4. Some small VPS can be obtained, not powerfull enough for the servers themself, but only for forwarding. I think there are some for under 5€), the connection is then done via wireguard. but its a bit configuration heavy to setup) 

Or you connect your friends via VPN to your net and play via local lan then.
Many windowsgsm plugin creators recommend zerotier (should be a free VPN designated for gaming) , see chapter below (or tailscale, but no howto there)

## How can you play with your friends without port forwarding?
- Use [zerotier](https://www.zerotier.com/) folow the basic guide and create network
- Download the client app and join to your network
- Create static IP address for your host machine
- Edit WGSM IP Address to your recently created static IP address
- Give your network ID to your friends
- After they've joined to your network
- They can connect using the IP you've created eg: 10.123.17.1:7777
- Enjoy


### Support
[WGSM](https://discord.com/channels/590590698907107340/645730252672335893)

### Give Love!
[Buy me a coffee](https://ko-fi.com/raziel7893)

[Paypal](https://paypal.me/raziel7893)

### License
This project is licensed under the MIT License - see the <a href="https://github.com/Heapons/WindowsGSM.TF2_Classified/blob/main/LICENSE">LICENSE.md</a> file for details
