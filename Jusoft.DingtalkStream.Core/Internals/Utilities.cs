using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace Jusoft.DingtalkStream.Core.Internals
{
    /// <summary>
    /// 内部辅助的工具类，不污染外部
    /// </summary>
    internal static class Utilities
    {
        /// <summary>
        ///  获取钉钉Stream回调网关的地址
        /// </summary>
        public const string DINGTALK_GATEWAY_ENDPOINT = "https://api.dingtalk.com/v1.0/gateway/connections/open";


        // Virtual 虚拟的
        // vmware VMWARE
        // Bluetooth 蓝牙
        static readonly IReadOnlyCollection<string> VirtualNetworkNames = new string[] { "vmnetadapter" , "Virtual" , "vmware" , "ppoe" , "bthpan" , "ndisip" , "sinforvnic" , "vpn" , "TeamViewer" , "Bluetooth" };
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
            var osName = Environment.OSVersion.Platform.ToString().Replace(" " , "");
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
                return $".NetFramework/{frameworkVersion.Major}.{frameworkVersion.Minor}";
            }
            // 判断是否为.NET Core 2
            else if (frameworkVersion.Major == 2 && frameworkVersion.Minor == 1)
            {
                return $".NetCore/{frameworkVersion.Major}.{frameworkVersion.Minor}";
            }
            // 判断是否为.NET Core 3
            else if (frameworkVersion.Major == 3 && frameworkVersion.Minor == 1)
            {
                return $".NetCore/{frameworkVersion.Major}.{frameworkVersion.Minor}";
            }
            else
            {
                return $".Net/{frameworkVersion.Major}.{frameworkVersion.Minor}";
            }
        }

        /// <summary>
        /// 获取本机本地IP 地址
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IPAddress> GetLocalIps()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();

            var list = new List<IPAddress>();

            foreach (var networkInterface in interfaces)
            {
                // 跳过未启用的网卡
                if (networkInterface.OperationalStatus != OperationalStatus.Up)
                    continue;
                // 跳过回环
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;
                // 跳过虚拟网卡
                if (VirtualNetworkNames.Any(networkName => networkInterface.Description.Contains(networkName , StringComparison.OrdinalIgnoreCase)))
                    continue;
                // 跳过没有速率的
                if (networkInterface.Speed < 0)
                    continue;

                var ipProperties = networkInterface.GetIPProperties();
                //ipProperties.GatewayAddresses;// 网关服务器地址
                //ipProperties.DhcpServerAddresses // DHCP 服务器地址
                //ipProperties.DnsAddresses //DNS 掩码信息
                //ipProperties.UnicastAddresses;// IP信息
                foreach (var unicastAddress in ipProperties.UnicastAddresses)
                {
                    // 判断如果是指示的任意地址，则跳过
                    if (unicastAddress.Address == IPAddress.Any)
                        continue;
                    //! 判断如果非IPV4 且 非IPV6 的地址，则跳过
                    if (unicastAddress.Address.AddressFamily != AddressFamily.InterNetwork || unicastAddress.Address.AddressFamily != AddressFamily.InterNetworkV6)
                        continue;

                    // 判断如果是苹果操作系统，则直接返回网卡地址，不进行 DuplicateAddressDetectionState 的校验【貌似苹果操作系统不支持】
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        // Unix 系统上的特定代码
                        yield return unicastAddress.Address;
                    }
                    // 其他操作系统情况则增加 DuplicateAddressDetectionState 的校验，仅取有效的地址
                    else if (getDuplicateAddressDetectionState(unicastAddress) == DuplicateAddressDetectionState.Preferred)
                    {
                        yield return unicastAddress.Address;
                    }

                    // 忽略其余情况，此时地址可能是无效的，或快过期的地址
                }
            }

            static DuplicateAddressDetectionState getDuplicateAddressDetectionState(UnicastIPAddressInformation unicastAddress)
            {
                try
                {
                    //issue@1 linux 下不支持 DuplicateAddressDetectionState 的问题修复
                    return unicastAddress.DuplicateAddressDetectionState;
                }
                catch
                {
                    // 如果上面的代码出现异常，则直接认为是有效的地址
                    return DuplicateAddressDetectionState.Preferred;
                }
            }
        }
    }
}
