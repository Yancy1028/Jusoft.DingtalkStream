using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Linq;

namespace Jusoft.DingtalkStream.Internals
{
    internal static class Utilities
    {
        /// <summary>
        ///  获取钉钉Stream回调网关的地址
        /// </summary>
        public const string DINGTALK_GATEWAY_ENDPOINT = "https://api.dingtalk.com/v1.0/gateway/connections/open";


        // Virtual 虚拟的
        // vmware VMWARE
        // Bluetooth 蓝牙
        static readonly IReadOnlyCollection<string> VirtualNetworkNames = new string[] { "vmnetadapter", "Virtual", "vmware", "ppoe", "bthpan", "ndisip", "sinforvnic", "vpn", "TeamViewer", "Bluetooth" };
        /// <summary>
        /// 获取当前SDK的版本信息
        /// </summary>
        /// <returns></returns>
        public static string GetSDKVersion()
        {
            var currentVersion = typeof(Utilities).Assembly.GetName().Version;
            return $"Jusoft.DingtalkStream/{currentVersion.Major}.{currentVersion.Minor}.{currentVersion.Build}";
        }
        /// <summary>
        /// 获取操作系统版本信息
        /// </summary>
        /// <returns></returns>
        public static string GetOSVersion()
        {
            var osName = Environment.OSVersion.Platform.ToString().Replace(" ", "");
            var osVersion = Environment.OSVersion.Version;
            return $"{osName}/{osVersion.Major}.{osVersion.Minor}";
        }

        /// <summary>
        /// 获取运行框架版本信息
        /// </summary>
        /// <returns></returns>
        public static string GetFrameworkVersion()
        {
            var frameworkVersion = Environment.Version;
            // 判断是否为 .NET Framework 4.x 或者为 .NET Framework 3.5
            if (frameworkVersion.Major == 4 || (frameworkVersion.Major == 3 && frameworkVersion.Minor == 5))
            {
                return $"NetFramework/{frameworkVersion.Major}.{frameworkVersion.Minor}";
            }
            // 判断是否为.NET Core 2
            else if (frameworkVersion.Major == 2 && frameworkVersion.Minor == 1)
            {
                return $"NetCore/{frameworkVersion.Major}.{frameworkVersion.Minor}";
            }
            // 判断是否为.NET Core 3
            else if (frameworkVersion.Major == 3 && frameworkVersion.Minor == 1)
            {
                return $"NetCore/{frameworkVersion.Major}.{frameworkVersion.Minor}";
            }
            else
            {
                return $"Net/{frameworkVersion.Major}.{frameworkVersion.Minor}";
            }
        }
        /// <summary>
        /// 获取本机本地IP 地址
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IPAddress> GetLocalIps()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            var list = new List<IPAddress>();

            foreach (var item in networkInterfaces)
            {
                if (item.Speed < 0) continue;// 排除没有速率的
                if (item.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;// 回环
                // 虚拟网卡
                if (VirtualNetworkNames.Any(networkName => item.Description.Contains(networkName, StringComparison.OrdinalIgnoreCase))) continue;

                var ip = item.GetIPProperties();
                //ip.GatewayAddresses;// 网关服务器地址
                //ip.DhcpServerAddresses // DHCP 服务器地址
                //ip.DnsAddresses //DNS 掩码信息
                //ip.UnicastAddresses;// IP信息
                foreach (var unicastAddress in ip.UnicastAddresses)
                {
                    if (unicastAddress.Address == IPAddress.Any) continue;
                    // 判断是否IPV4的地址
                    if (unicastAddress.Address.AddressFamily == AddressFamily.InterNetwork
                        && unicastAddress.DuplicateAddressDetectionState == DuplicateAddressDetectionState.Preferred)
                    {
                        yield return unicastAddress.Address;
                    }

                }
            }

        }
    }
}
