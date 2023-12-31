﻿using GalacticLib.Objects;
using GalacticLib.Semantic;
using Shell.NET;
using System.Text.RegularExpressions;

namespace GalacticLib.Platforms.Linux;
/// <summary> Determine linux distro information </summary>
public static class Distro {
    public static DistroName FromID(string id)
        => id.ToLower() switch {
            "alpine" => DistroName.AlpineLinux,
            "amzn" => DistroName.AmazonLinux,
            "arch" => DistroName.Arch,
            "centos" => DistroName.CentOS7,
            "debian" => DistroName.Debian,
            "elementary" => DistroName.ElementaryOS,
            "fedora" => DistroName.Fedora,
            "linuxmint" => DistroName.LinuxMint,
            "ol" => DistroName.OracleLinux,
            "neon" => DistroName.KDENeon,

            "opensuse" => DistroName.OpenSUSE,
            "opensuse-leap" => DistroName.OpenSUSE,
            "suse opensuse" => DistroName.OpenSUSE,

            "rhel fedora" => DistroName.Fedora,
            "sparky" => DistroName.SparkyLinux,
            "suse" => DistroName.SUSE,
            "ubuntu" => DistroName.Ubuntu,
            "ubuntu debian" => DistroName.Ubuntu,
            "zorin" => DistroName.ZorinOS,

            _ => DistroName.Other
        };

    public static DEName DEFromID(string id)
        => id.Parse<DEName>(ignoreCase: true);

    /// <summary> Information about the currently running Linux OS </summary>
    public static class Current {
        private static Bash _Bash = new();
        private static Bash Bash => _Bash ??= new();

#nullable enable
        public static string? @XDG_SESSION_TYPE => Environment.GetEnvironmentVariable("XDG_SESSION_TYPE");
        public static string? @XDG_SESSION_DESKTOP => Environment.GetEnvironmentVariable("XDG_SESSION_DESKTOP");
#nullable restore

        /// <summary> Indicates whether this linux is running in WSL (Windows Subsystem for Linux) </summary>
        public static bool OnWSL
            => Regex.IsMatch(Bash.Command("uname").Output, "(microsoft|wsl)", RegexOptions.IgnoreCase);
        public static bool OnWayland
            => @XDG_SESSION_TYPE is string sessionType
            && sessionType.Equals("wayland", StringComparison.OrdinalIgnoreCase);
        public static bool OnXOrg
            => @XDG_SESSION_TYPE is string sessionType
            && sessionType.Equals("xorg", StringComparison.OrdinalIgnoreCase);
        public static DEName DesktopEnvironment
            => @XDG_SESSION_DESKTOP is string sessionDesktop
            ? DEFromID(sessionDesktop)
            : DEName.Other;

        /// <summary> <c> uname -r </c> <br/>
        ///     Kernel version <br/><br/>
        ///     # Example: 5.15.0-58-generic (as <see cref="SemanticVersion"/>)
        /// </summary>
        public static SemanticVersion KernelVersion
            => new(Bash.Command("uname -r").Output, false);
        private static string _ParseReleaseCommandString(string variable)
            => $"grep -w {variable} /etc/*-release | cut -d= -f2";
        private static string _Parse_Run_RemoveQuotations(string variable)
            => RemoveQuotations(Bash.Command(_ParseReleaseCommandString(variable)).Output);
        private static string RemoveQuotations(string input)
            => Regex.IsMatch(input, "^\".*\"$")
            ? input[1..(input.Length - 1)]
            : input;

        /// <summary> <c> >> /etc/*-release >> $ID </c> <br/>
        ///     ID of this distro <br/><br/>
        ///     # Example: ubuntu
        /// </summary>
        public static DistroName ID
            => FromID(_Parse_Run_RemoveQuotations("ID"));

        /// <summary> <c> >> /etc/*-release >> $ID_LIKE </c> <br/>
        ///     All IDs of the distros this is based on <br/><br/>
        ///     # Example: debian (<see cref="string"/>)
        /// </summary>
        public static List<DistroName> BaseIDs
            => _Parse_Run_RemoveQuotations("ID_LIKE").Split(' ')
            .Aggregate(
                new List<DistroName>(),
                (list, id) => {
                    list.Add(FromID(id));
                    return list;
                }
            );
        /// <summary> <c> >> /etc/*-release >> $ID_LIKE </c> <br/>
        ///     ID of the distro this is based on <br/><br/>
        ///     # Example: debian (<see cref="string"/>)
        /// </summary>
        public static DistroName BaseID => BaseIDs[0];

        /// <summary> <c> >> /etc/*-release >> $VERSION_ID </c> <br/>
        ///     Version presented in a numerical way <br/><br/>
        ///     # Example: 20.04 (as <see cref="SemanticVersion"/>) <br/>
        ///     <br/>
        ///  !!!  Except Arch and CentOS 5~6 AND maybe others ¯\_(ツ)_/¯
        /// </summary>
        public static SemanticVersion Version
            => new(_Parse_Run_RemoveQuotations("VERSION_ID"));

        /// <summary> <c> >> /etc/*-release >> $NAME </c> <br/>
        ///     Short Name of this distro <br/><br/>
        ///     # Example: Debian GNU/Linux 11 (bullseye)
        /// </summary>
        public static string Name
            => _Parse_Run_RemoveQuotations("NAME");

        /// <summary> <c> >> /etc/*-release >> $PRETTY_NAME </c> <br/>
        ///     Long Name of this distro <br/><br/>
        ///     # Example: Debian GNU/Linux
        /// </summary>
        public static string PrettyName
            => _Parse_Run_RemoveQuotations("PRETTY_NAME");

        /// <summary> <c> >> /etc/*-release >> $HOME_URL </c> <br/>
        ///     Home page URL (<see cref="string"/>) of this distro <br/><br/>
        ///     # Example: https://www.ubuntu.com/ (as <see cref="string"/>)
        /// </summary>
        public static string Homepage
            => _Parse_Run_RemoveQuotations("HOME_URL");

    }
}


//!? TESTS
//? >> $ID is common (All)
//? >> $VERSION_ID is common (Except Arch || CentOS 6~5)

//? >> Can be used
//? $ . /etc/*-release && echo $name
//! (output: value of name)

//!? Debian 11 ======================================
//? $ cat /etc/*-release
//  PRETTY_NAME = "Debian GNU/Linux 11 (bullseye)"
//  NAME = "Debian GNU/Linux"
//  VERSION_ID = "11"
//  VERSION = "11 (bullseye)"
//  VERSION_CODENAME = bullseye
//  ID = debian
//  HOME_URL = "https://www.debian.org/"
//  SUPPORT_URL = "https://www.debian.org/support"
//  BUG_REPORT_URL = "https://bugs.debian.org/"
//!? ================================================

//!? Ubuntu 20.04 ===================================
//? $ cat /etc/*-release
//  DISTRIB_ID=Ubuntu
//  DISTRIB_RELEASE=20.04
//  DISTRIB_CODENAME=focal
//  DISTRIB_DESCRIPTION = "Ubuntu 20.04 LTS"
//  NAME="Ubuntu"
//  VERSION="20.04 LTS (Focal Fossa)"
//  ID=ubuntu
//  ID_LIKE = debian
//  PRETTY_NAME="Ubuntu 20.04 LTS"
//  VERSION_ID="20.04"
//  HOME_URL="https://www.ubuntu.com/"
//  SUPPORT_URL="https://help.ubuntu.com/"
//  BUG_REPORT_URL="https://bugs.launchpad.net/ubuntu/"
//  PRIVACY_POLICY_URL="https://www.ubuntu.com/legal/terms-and-policies/privacy-policy"
//  VERSION_CODENAME=focal
//  UBUNTU_CODENAME = focal
//!? ================================================

//!? KDE Neon v5.22 =================================
//? $ cat /etc/*-release
//  DISTRIB_ID=neon
//  DISTRIB_RELEASE=20.04
//  DISTRIB_CODENAME=focal
//  DISTRIB_DESCRIPTION="KDE neon User Edition 5.22"
//  NAME="KDE neon"
//  VERSION="5.22"
//  ID=neon
//  ID_LIKE="ubuntu debian"
//  PRETTY_NAME="KDE neon User Edition 5.22"
//  VARIANT="User Edition"
//  VARIANT_ID=user
//  VERSION_ID="20.04"
//  HOME_URL="https://neon.kde.org/"
//  SUPPORT_URL="https://neon.kde.org/"
//. BUG_REPORT_URL="https://bugs.kde.org/"
//  LOGO=start-here-kde-neon
//  PRIVACY_POLICY_URL="https://www.ubuntu.com/legal/terms-and-policies/privacy-policy"
//  VERSION_CODENAME=focal
//  UBUNTU_CODENAME=focal

// ! $ cat /etc/lsb-release
//  DISTRIB_ID=neon
//  DISTRIB_RELEASE=20.04
//  DISTRIB_CODENAME=focal
//  DISTRIB_DESCRIPTION="KDE neon User Edition 5.22"
//!? ================================================

//!? Arch v?? =======================================
//? $ cat /etc/os-release
//  NAME="Arch Linux"
//  ID=arch
//  PRETTY_NAME="Arch Linux"
//  ANSI_COLOR="0;36"
//  HOME_URL="https://www.archlinux.org/"
//  SUPPORT_URL="https://bbs.archlinux.org/"
//  BUG_REPORT_URL="https://bugs.archlinux.org/"
//
//! Not installed by default! `lsb-release`
//? $ cat /etc/lsb-release
//  LSB_VERSION=1.4-14
//  DISTRIB_ID=Arch
//  DISTRIB_RELEASE = rolling
//  DISTRIB_DESCRIPTION="Arch Linux"
//
//! Empty: `/etc/arch-version`
//!? ================================================

//!? Amazon Linux 2016.09 ===========================
//? $ cat /etc/os-release
//  NAME="Amazon Linux AMI"
//  VERSION="2016.09"
//  ID="amzn"
//  ID_LIKE="rhel fedora"
//  VERSION_ID="2016.09"
//  PRETTY_NAME="Amazon Linux AMI 2016.09"
//  ANSI_COLOR="0;33"
//  CPE_NAME="cpe:/o:amazon:linux:2016.09:ga"
//  HOME_URL="http://aws.amazon.com/amazon-linux-ami/"
//
//! Empty: cat /etc/lsb-release
//
//? $ cat /etc/system-release
//  Amazon Linux AMI release 2016.09
//!? ================================================

//!? CentOS 7 =======================================
//? $ cat /etc/os-release
//  NAME="CentOS Linux"
//  VERSION="7 (Core)"
//  ID="centos"
//  ID_LIKE="rhel fedora"
//  VERSION_ID="7"
//  PRETTY_NAME="CentOS Linux 7 (Core)"
//  ANSI_COLOR="0;31"
//  CPE_NAME="cpe:/o:centos:centos:7"
//  HOME_URL="https://www.centos.org/"
//  BUG_REPORT_URL="https://bugs.centos.org/"
//
//  CENTOS_MANTISBT_PROJECT="CentOS-7"
//  CENTOS_MANTISBT_PROJECT_VERSION="7"
//  REDHAT_SUPPORT_PRODUCT="centos"
//  REDHAT_SUPPORT_PRODUCT_VERSION="7"
//
//! Does not exist: /etc/lsb-release
//!? ================================================

//!? CentOS 6 and 5 =================================
//! Does not exist: /etc/os-release
//
//? $ cat /etc/lsb-release
//  LSB_VERSION=base-4.0-amd64:base-4.0-noarch:core-4.0-amd64:core-4.0-noarch
//
//? $ cat /etc/centos-release
//  CentOS release 6.7 (Final)
//!? ================================================

//!? Fedora 22 ======================================
//? $ cat  /etc/os-release
//  NAME=Fedora
//  VERSION="22 (Twenty Two)"
//  ID=fedora
//  VERSION_ID = 22
//  PRETTY_NAME="Fedora 22 (Twenty Two)"
//  ANSI_COLOR="0;34"
//  CPE_NAME="cpe:/o:fedoraproject:fedora:22"
//  HOME_URL="https://fedoraproject.org/"
//  BUG_REPORT_URL="https://bugzilla.redhat.com/"
//  REDHAT_BUGZILLA_PRODUCT="Fedora"
//  REDHAT_BUGZILLA_PRODUCT_VERSION=22
//  REDHAT_SUPPORT_PRODUCT="Fedora"
//  REDHAT_SUPPORT_PRODUCT_VERSION=22
//  PRIVACY_POLICY_URL=https://fedoraproject.org/wiki/Legal:PrivacyPolicy
//
//! Does not exist: /etc/lsb-release
//
//? $ cat /etc/fedora-release
//  Fedora release 22 (Twenty Two)
//!? ================================================

//!? openSUSE Tumbleweed ============================
//? $ cat  /etc/os-release
//  NAME=openSUSE
//  VERSION="20150725 (Tumbleweed)"
//  VERSION_ID="20150725"
//  PRETTY_NAME="openSUSE 20150725 (Tumbleweed) (x86_64)"
//  ID=opensuse
//  ANSI_COLOR = "0;32"
//  CPE_NAME="cpe:/o:opensuse:opensuse:20150725"
//  BUG_REPORT_URL="https://bugs.opensuse.org"
//  HOME_URL="https://opensuse.org/"
//  ID_LIKE="suse"
//
//! Does not exist: /etc/lsb-release
//
//? $ cat /etc/SuSE-release
//  openSUSE 20150725 (x86_64)
//  VERSION = 20150725
//  CODENAME = Tumbleweed
//  # /etc/SuSE-release is deprecated and will be removed in the future, use /etc/os-release instead
//!? ================================================
