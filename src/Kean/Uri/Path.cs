// Copyright (C) 2010-2012  Simon Mika <simon@mika.se>
//
// This file is part of Kean.
//
// Kean is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kean is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.  If not, see <http://www.gnu.org/licenses/>.
//


using System;
using Kean.Extension;
using Kean.Collection.Extension;

namespace Kean.Uri
{
	public class Path :
		Collection.Linked.List<PathLink, string>,
		IEquatable<Path>
	{
		#region IString Members
		public string String
		{
			get
			{
				string result = "";
				PathLink tail = this.Last;
				while (tail.NotNull())
				{
					result = "/" + tail.Head.PercentEncode(' ', '%', '/', '#', '?') + result;
					tail = tail.Tail;
				}
				if (result.StartsWith("/."))
					result = result.TrimStart('/');
				return result;
			}
			set
			{
				PathLink tail = null;
				if (value.NotNull())
					foreach (string folder in value.TrimStart('/').Split('/'))
						tail = new PathLink(folder.PercentDecode(), tail);
				this.Last = tail;
			}
		}
		#endregion
		public string PlatformPath
		{
			get
			{
				string result = "";
				PathLink tail = this.Last;
				if (tail.NotNull())
				{
					string name;
					while (tail.NotNull())
					{
						name = tail.Head ?? "";
						if (tail.Tail.IsNull() && name.EndsWith(":"))
							name += System.IO.Path.DirectorySeparatorChar;
						result = System.IO.Path.Combine(name, result);
						tail = tail.Tail;
					}
					if (System.IO.Path.DirectorySeparatorChar == '/' && !result.StartsWith("."))
						result = System.IO.Path.DirectorySeparatorChar + result;
				}
				return result;
			}
			set
			{
				if (value.NotEmpty() && (value[0] == System.IO.Path.DirectorySeparatorChar || value[0] == System.IO.Path.AltDirectorySeparatorChar))
					value = value.Substring(1);
				PathLink tail = null;
				if (value.NotNull())
					foreach (string folder in value.Split(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar))
						tail = new PathLink(folder, tail);
				this.Last = tail;
			}
		}
		public bool Empty { get { return this.Last.IsNull(); } }
		public bool Folder { get { return this.Last.NotNull() && this.Last.Head.IsEmpty(); } }
		public Path FolderPath { get { return this.Last.NotNull() ? new Path(this.Last.Tail) : new Path(); } }
		public string Stem
		{
			get { return this.Last.NotNull() ? this.Last.Head.Get(0, -this.Extension.Length - 1) : null; }
			set
			{
				if (this.Last.NotNull())
					this.Last = new PathLink();
				this.Last.Head = value + "." + this.Extension;
			}
		}
		public string Filename
		{
			get { return this.Last.NotNull() ? this.Last.Head : null; }
			set
			{
				if (this.Last.NotNull())
					this.Last = new PathLink();
				this.Last.Head = value;
			}
		}
		public string Extension
		{
			get { return this.Last.NotNull() ? this.Last.Head.Split('.').Skip(1).Last() : null; }
			set
			{
				if (this.Last.NotNull())
					this.Last = new PathLink();
				this.Last.Head = this.Stem + "." + value;
			}
		}
		/// <summary>
		/// Deduces the MIME type from the extension.
		/// </summary>
		/// <value>The MIME type if.</value>
		public string Mime
		{
			get
			{
				string result;
				switch (this.Extension)
				{
					#region extension - mime mapping
					case "323":
						result = "text/h323";
						break;
					case "3dmf":
						result = "x-world/x-3dmf";
						break;
					case "3dm":
						result = "x-world/x-3dmf";
						break;
					case "aab":
						result = "application/x-authorware-bin";
						break;
					case "aam":
						result = "application/x-authorware-map";
						break;
					case "aas":
						result = "application/x-authorware-seg";
						break;
					case "abc":
						result = "text/vnd.abc";
						break;
					case "acgi":
						result = "text/html";
						break;
					case "acx":
						result = "application/internet-property-stream";
						break;
					case "afl":
						result = "video/animaflex";
						break;
					case "ai":
						result = "application/postscript";
						break;
					case "aif":
						result = "audio/aiff";
						break;
					case "aifc":
						result = "audio/aiff";
						break;
					case "aiff":
						result = "audio/aiff";
						break;
					case "aim":
						result = "application/x-aim";
						break;
					case "aip":
						result = "text/x-audiosoft-intra";
						break;
					case "ani":
						result = "application/x-navi-animation";
						break;
					case "aos":
						result = "application/x-nokia-9000-communicator-add-on-software";
						break;
					case "application":
						result = "application/x-ms-application";
						break;
					case "aps":
						result = "application/mime";
						break;
					case "art":
						result = "image/x-jg";
						break;
					case "asf":
						result = "video/x-ms-asf";
						break;
					case "asm":
						result = "text/x-asm";
						break;
					case "asp":
						result = "text/asp";
						break;
					case "asr":
						result = "video/x-ms-asf";
						break;
					case "asx":
						result = "application/x-mplayer2";
						break;
					case "au":
						result = "audio/x-au";
						break;
					case "avi":
						result = "video/avi";
						break;
					case "avs":
						result = "video/avs-video";
						break;
					case "axs":
						result = "application/olescript";
						break;
					case "bas":
						result = "text/plain";
						break;
					case "bcpio":
						result = "application/x-bcpio";
						break;
					case "bin":
						result = "application/octet-stream";
						break;
					case "bm":
						result = "image/bmp";
						break;
					case "bmp":
						result = "image/bmp";
						break;
					case "boo":
						result = "application/book";
						break;
					case "book":
						result = "application/book";
						break;
					case "boz":
						result = "application/x-bzip2";
						break;
					case "bsh":
						result = "application/x-bsh";
						break;
					case "bz2":
						result = "application/x-bzip2";
						break;
					case "bz":
						result = "application/x-bzip";
						break;
					case "cat":
						result = "application/vnd.ms-pki.seccat";
						break;
					case "ccad":
						result = "application/clariscad";
						break;
					case "cco":
						result = "application/x-cocoa";
						break;
					case "cc":
						result = "text/plain";
						break;
					case "cdf":
						result = "application/cdf";
						break;
					case "cer":
						result = "application/pkix-cert";
						break;
					case "cha":
						result = "application/x-chat";
						break;
					case "chat":
						result = "application/x-chat";
						break;
					case "class":
						result = "application/java";
						break;
					case "clp":
						result = "application/x-msclip";
						break;
					case "cmx":
						result = "image/x-cmx";
						break;
					case "cod":
						result = "image/cis-cod";
						break;
					case "conf":
						result = "text/plain";
						break;
					case "cpio":
						result = "application/x-cpio";
						break;
					case "cpp":
						result = "text/plain";
						break;
					case "cpt":
						result = "application/x-cpt";
						break;
					case "crd":
						result = "application/x-mscardfile";
						break;
					case "crl":
						result = "application/pkix-crl";
						break;
					case "crt":
						result = "application/pkix-cert";
						break;
					case "csh":
						result = "application/x-csh";
						break;
					case "css":
						result = "text/css";
						break;
					case "c":
						result = "text/plain";
						break;
					case "c++":
						result = "text/plain";
						break;
					case "cxx":
						result = "text/plain";
						break;
					case "dcr":
						result = "application/x-director";
						break;
					case "deepv":
						result = "application/x-deepv";
						break;
					case "def":
						result = "text/plain";
						break;
					case "deploy":
						result = "application/octet-stream";
						break;
					case "der":
						result = "application/x-x509-ca-cert";
						break;
					case "dib":
						result = "image/bmp";
						break;
					case "dif":
						result = "video/x-dv";
						break;
					case "dir":
						result = "application/x-director";
						break;
					case "disco":
						result = "text/xml";
						break;
					case "dll":
						result = "application/x-msdownload";
						break;
					case "dl":
						result = "video/dl";
						break;
					case "doc":
						result = "application/msword";
						break;
					case "dot":
						result = "application/msword";
						break;
					case "dp":
						result = "application/commonground";
						break;
					case "drw":
						result = "application/drafting";
						break;
					case "dvi":
						result = "application/x-dvi";
						break;
					case "dv":
						result = "video/x-dv";
						break;
					case "dwf":
						result = "drawing/x-dwf (old)";
						break;
					case "dwg":
						result = "application/acad";
						break;
					case "dxf":
						result = "application/dxf";
						break;
					case "dxr":
						result = "application/x-director";
						break;
					case "elc":
						result = "application/x-elc";
						break;
					case "el":
						result = "text/x-script.elisp";
						break;
					case "eml":
						result = "message/rfc822";
						break;
					case "eot":
						result = "application/vnd.bw-fontobject";
						break;
					case "eps":
						result = "application/postscript";
						break;
					case "es":
						result = "application/x-esrehber";
						break;
					case "etx":
						result = "text/x-setext";
						break;
					case "evy":
						result = "application/envoy";
						break;
					case "exe":
						result = "application/octet-stream";
						break;
					case "f77":
						result = "text/plain";
						break;
					case "f90":
						result = "text/plain";
						break;
					case "fdf":
						result = "application/vnd.fdf";
						break;
					case "fif":
						result = "image/fif";
						break;
					case "fli":
						result = "video/fli";
						break;
					case "flo":
						result = "image/florian";
						break;
					case "flr":
						result = "x-world/x-vrml";
						break;
					case "flx":
						result = "text/vnd.fmi.flexstor";
						break;
					case "fmf":
						result = "video/x-atomic3d-feature";
						break;
					case "for":
						result = "text/plain";
						break;
					case "fpx":
						result = "image/vnd.fpx";
						break;
					case "frl":
						result = "application/freeloader";
						break;
					case "f":
						result = "text/plain";
						break;
					case "funk":
						result = "audio/make";
						break;
					case "g3":
						result = "image/g3fax";
						break;
					case "gif":
						result = "image/gif";
						break;
					case "gl":
						result = "video/gl";
						break;
					case "gsd":
						result = "audio/x-gsm";
						break;
					case "gsm":
						result = "audio/x-gsm";
						break;
					case "gsp":
						result = "application/x-gsp";
						break;
					case "gss":
						result = "application/x-gss";
						break;
					case "gtar":
						result = "application/x-gtar";
						break;
					case "g":
						result = "text/plain";
						break;
					case "gz":
						result = "application/x-gzip";
						break;
					case "gzip":
						result = "application/x-gzip";
						break;
					case "hdf":
						result = "application/x-hdf";
						break;
					case "help":
						result = "application/x-helpfile";
						break;
					case "hgl":
						result = "application/vnd.hp-HPGL";
						break;
					case "hh":
						result = "text/plain";
						break;
					case "hlb":
						result = "text/x-script";
						break;
					case "hlp":
						result = "application/x-helpfile";
						break;
					case "hpg":
						result = "application/vnd.hp-HPGL";
						break;
					case "hpgl":
						result = "application/vnd.hp-HPGL";
						break;
					case "hqx":
						result = "application/binhex";
						break;
					case "hta":
						result = "application/hta";
						break;
					case "htc":
						result = "text/x-component";
						break;
					case "h":
						result = "text/plain";
						break;
					case "htmls":
						result = "text/html";
						break;
					case "html":
						result = "text/html";
						break;
					case "htm":
						result = "text/html";
						break;
					case "htt":
						result = "text/webviewhtml";
						break;
					case "htx":
						result = "text/html";
						break;
					case "ice":
						result = "x-conference/x-cooltalk";
						break;
					case "ico":
						result = "image/x-icon";
						break;
					case "idc":
						result = "text/plain";
						break;
					case "ief":
						result = "image/ief";
						break;
					case "iefs":
						result = "image/ief";
						break;
					case "iges":
						result = "application/iges";
						break;
					case "igs":
						result = "application/iges";
						break;
					case "iii":
						result = "application/x-iphone";
						break;
					case "ima":
						result = "application/x-ima";
						break;
					case "imap":
						result = "application/x-httpd-imap";
						break;
					case "inf":
						result = "application/inf";
						break;
					case "ins":
						result = "application/x-internett-signup";
						break;
					case "ip":
						result = "application/x-ip2";
						break;
					case "isp":
						result = "application/x-internet-signup";
						break;
					case "isu":
						result = "video/x-isvideo";
						break;
					case "it":
						result = "audio/it";
						break;
					case "iv":
						result = "application/x-inventor";
						break;
					case "ivf":
						result = "video/x-ivf";
						break;
					case "ivr":
						result = "i-world/i-vrml";
						break;
					case "ivy":
						result = "application/x-livescreen";
						break;
					case "jam":
						result = "audio/x-jam";
						break;
					case "java":
						result = "text/plain";
						break;
					case "jav":
						result = "text/plain";
						break;
					case "jcm":
						result = "application/x-java-commerce";
						break;
					case "jfif":
						result = "image/jpeg";
						break;
					case "jfif-tbnl":
						result = "image/jpeg";
						break;
					case "jpeg":
						result = "image/jpeg";
						break;
					case "jpe":
						result = "image/jpeg";
						break;
					case "jpg":
						result = "image/jpeg";
						break;
					case "jps":
						result = "image/x-jps";
						break;
					case "js":
						result = "application/x-javascript";
						break;
					case "jut":
						result = "image/jutvision";
						break;
					case "kar":
						result = "audio/midi";
						break;
					case "ksh":
						result = "text/x-script.ksh";
						break;
					case "la":
						result = "audio/nspaudio";
						break;
					case "lam":
						result = "audio/x-liveaudio";
						break;
					case "latex":
						result = "application/x-latex";
						break;
					case "list":
						result = "text/plain";
						break;
					case "lma":
						result = "audio/nspaudio";
						break;
					case "log":
						result = "text/plain";
						break;
					case "lsp":
						result = "application/x-lisp";
						break;
					case "lst":
						result = "text/plain";
						break;
					case "lsx":
						result = "text/x-la-asf";
						break;
					case "ltx":
						result = "application/x-latex";
						break;
					case "m13":
						result = "application/x-msmediaview";
						break;
					case "m14":
						result = "application/x-msmediaview";
						break;
					case "m1v":
						result = "video/mpeg";
						break;
					case "m2a":
						result = "audio/mpeg";
						break;
					case "m2v":
						result = "video/mpeg";
						break;
					case "m3u":
						result = "audio/x-mpequrl";
						break;
					case "man":
						result = "application/x-troff-man";
						break;
					case "manifest":
						result = "application/x-ms-manifest";
						break;
					case "map":
						result = "application/x-navimap";
						break;
					case "mar":
						result = "text/plain";
						break;
					case "mbd":
						result = "application/mbedlet";
						break;
					case "mc$":
						result = "application/x-magic-cap-package-1.0";
						break;
					case "mcd":
						result = "application/mcad";
						break;
					case "mcf":
						result = "image/vasa";
						break;
					case "mcp":
						result = "application/netmc";
						break;
					case "mdb":
						result = "application/x-msaccess";
						break;
					case "me":
						result = "application/x-troff-me";
						break;
					case "mht":
						result = "message/rfc822";
						break;
					case "mhtml":
						result = "message/rfc822";
						break;
					case "mid":
						result = "audio/midi";
						break;
					case "midi":
						result = "audio/midi";
						break;
					case "mif":
						result = "application/x-mif";
						break;
					case "mime":
						result = "message/rfc822";
						break;
					case "mjf":
						result = "audio/x-vnd.AudioExplosion.MjuiceMediaFile";
						break;
					case "mjpg":
						result = "video/x-motion-jpeg";
						break;
					case "mm":
						result = "application/base64";
						break;
					case "mme":
						result = "application/base64";
						break;
					case "mny":
						result = "application/x-msmoney";
						break;
					case "mod":
						result = "audio/mod";
						break;
					case "moov":
						result = "video/quicktime";
						break;
					case "movie":
						result = "video/x-sgi-movie";
						break;
					case "mov":
						result = "video/quicktime";
						break;
					case "mp2":
						result = "video/mpeg";
						break;
					case "mp3":
						result = "audio/mpeg3";
						break;
					case "mpa":
						result = "audio/mpeg";
						break;
					case "mpc":
						result = "application/x-project";
						break;
					case "mpeg":
						result = "video/mpeg";
						break;
					case "mpe":
						result = "video/mpeg";
						break;
					case "mpga":
						result = "audio/mpeg";
						break;
					case "mpg":
						result = "video/mpeg";
						break;
					case "mpp":
						result = "application/vnd.ms-project";
						break;
					case "mpt":
						result = "application/x-project";
						break;
					case "mpv2":
						result = "video/mpeg";
						break;
					case "mpv":
						result = "application/x-project";
						break;
					case "mpx":
						result = "application/x-project";
						break;
					case "mrc":
						result = "application/marc";
						break;
					case "ms":
						result = "application/x-troff-ms";
						break;
					case "m":
						result = "text/plain";
						break;
					case "mvb":
						result = "application/x-msmediaview";
						break;
					case "mv":
						result = "video/x-sgi-movie";
						break;
					case "my":
						result = "audio/make";
						break;
					case "mzz":
						result = "application/x-vnd.AudioExplosion.mzz";
						break;
					case "nap":
						result = "image/naplps";
						break;
					case "naplps":
						result = "image/naplps";
						break;
					case "nc":
						result = "application/x-netcdf";
						break;
					case "ncm":
						result = "application/vnd.nokia.configuration-message";
						break;
					case "niff":
						result = "image/x-niff";
						break;
					case "nif":
						result = "image/x-niff";
						break;
					case "nix":
						result = "application/x-mix-transfer";
						break;
					case "nsc":
						result = "application/x-conference";
						break;
					case "nvd":
						result = "application/x-navidoc";
						break;
					case "nws":
						result = "message/rfc822";
						break;
					case "oda":
						result = "application/oda";
						break;
					case "ods":
						result = "application/oleobject";
						break;
					case "omc":
						result = "application/x-omc";
						break;
					case "omcd":
						result = "application/x-omcdatamaker";
						break;
					case "omcr":
						result = "application/x-omcregerator";
						break;
					case "p10":
						result = "application/pkcs10";
						break;
					case "p12":
						result = "application/pkcs-12";
						break;
					case "p7a":
						result = "application/x-pkcs7-signature";
						break;
					case "p7b":
						result = "application/x-pkcs7-certificates";
						break;
					case "p7c":
						result = "application/pkcs7-mime";
						break;
					case "p7m":
						result = "application/pkcs7-mime";
						break;
					case "p7r":
						result = "application/x-pkcs7-certreqresp";
						break;
					case "p7s":
						result = "application/pkcs7-signature";
						break;
					case "part":
						result = "application/pro_eng";
						break;
					case "pas":
						result = "text/pascal";
						break;
					case "pbm":
						result = "image/x-portable-bitmap";
						break;
					case "pcl":
						result = "application/x-pcl";
						break;
					case "pct":
						result = "image/x-pict";
						break;
					case "pcx":
						result = "image/x-pcx";
						break;
					case "pdb":
						result = "chemical/x-pdb";
						break;
					case "pdf":
						result = "application/pdf";
						break;
					case "pfunk":
						result = "audio/make";
						break;
					case "pfx":
						result = "application/x-pkcs12";
						break;
					case "pgm":
						result = "image/x-portable-graymap";
						break;
					case "pic":
						result = "image/pict";
						break;
					case "pict":
						result = "image/pict";
						break;
					case "pkg":
						result = "application/x-newton-compatible-pkg";
						break;
					case "pko":
						result = "application/vnd.ms-pki.pko";
						break;
					case "pl":
						result = "text/plain";
						break;
					case "plx":
						result = "application/x-PiXCLscript";
						break;
					case "pm4":
						result = "application/x-pagemaker";
						break;
					case "pm5":
						result = "application/x-pagemaker";
						break;
					case "pma":
						result = "application/x-perfmon";
						break;
					case "pmc":
						result = "application/x-perfmon";
						break;
					case "pm":
						result = "image/x-xpixmap";
						break;
					case "pml":
						result = "application/x-perfmon";
						break;
					case "pmr":
						result = "application/x-perfmon";
						break;
					case "pmw":
						result = "application/x-perfmon";
						break;
					case "png":
						result = "image/png";
						break;
					case "pnm":
						result = "application/x-portable-anymap";
						break;
					case "pot":
						result = "application/mspowerpoint";
						break;
					case "pov":
						result = "model/x-pov";
						break;
					case "ppa":
						result = "application/vnd.ms-powerpoint";
						break;
					case "ppm":
						result = "image/x-portable-pixmap";
						break;
					case "pps":
						result = "application/mspowerpoint";
						break;
					case "ppt":
						result = "application/mspowerpoint";
						break;
					case "ppz":
						result = "application/mspowerpoint";
						break;
					case "pre":
						result = "application/x-freelance";
						break;
					case "prf":
						result = "application/pics-rules";
						break;
					case "prt":
						result = "application/pro_eng";
						break;
					case "ps":
						result = "application/postscript";
						break;
					case "p":
						result = "text/x-pascal";
						break;
					case "pub":
						result = "application/x-mspublisher";
						break;
					case "pvu":
						result = "paleovu/x-pv";
						break;
					case "pwz":
						result = "application/vnd.ms-powerpoint";
						break;
					case "pyc":
						result = "applicaiton/x-bytecode.python";
						break;
					case "py":
						result = "text/x-script.phyton";
						break;
					case "qcp":
						result = "audio/vnd.qcelp";
						break;
					case "qd3d":
						result = "x-world/x-3dmf";
						break;
					case "qd3":
						result = "x-world/x-3dmf";
						break;
					case "qif":
						result = "image/x-quicktime";
						break;
					case "qtc":
						result = "video/x-qtc";
						break;
					case "qtif":
						result = "image/x-quicktime";
						break;
					case "qti":
						result = "image/x-quicktime";
						break;
					case "qt":
						result = "video/quicktime";
						break;
					case "ra":
						result = "audio/x-pn-realaudio";
						break;
					case "ram":
						result = "audio/x-pn-realaudio";
						break;
					case "ras":
						result = "application/x-cmu-raster";
						break;
					case "rast":
						result = "image/cmu-raster";
						break;
					case "rexx":
						result = "text/x-script.rexx";
						break;
					case "rf":
						result = "image/vnd.rn-realflash";
						break;
					case "rgb":
						result = "image/x-rgb";
						break;
					case "rm":
						result = "application/vnd.rn-realmedia";
						break;
					case "rmi":
						result = "audio/mid";
						break;
					case "rmm":
						result = "audio/x-pn-realaudio";
						break;
					case "rmp":
						result = "audio/x-pn-realaudio";
						break;
					case "rng":
						result = "application/ringing-tones";
						break;
					case "rnx":
						result = "application/vnd.rn-realplayer";
						break;
					case "roff":
						result = "application/x-troff";
						break;
					case "rp":
						result = "image/vnd.rn-realpix";
						break;
					case "rpm":
						result = "audio/x-pn-realaudio-plugin";
						break;
					case "rss":
						result = "text/xml";
						break;
					case "rtf":
						result = "text/richtext";
						break;
					case "rt":
						result = "text/richtext";
						break;
					case "rtx":
						result = "text/richtext";
						break;
					case "rv":
						result = "video/vnd.rn-realvideo";
						break;
					case "s3m":
						result = "audio/s3m";
						break;
					case "sbk":
						result = "application/x-tbook";
						break;
					case "scd":
						result = "application/x-msschedule";
						break;
					case "scm":
						result = "application/x-lotusscreencam";
						break;
					case "sct":
						result = "text/scriptlet";
						break;
					case "sdml":
						result = "text/plain";
						break;
					case "sdp":
						result = "application/sdp";
						break;
					case "sdr":
						result = "application/sounder";
						break;
					case "sea":
						result = "application/sea";
						break;
					case "set":
						result = "application/set";
						break;
					case "setpay":
						result = "application/set-payment-initiation";
						break;
					case "setreg":
						result = "application/set-registration-initiation";
						break;
					case "sgml":
						result = "text/sgml";
						break;
					case "sgm":
						result = "text/sgml";
						break;
					case "shar":
						result = "application/x-bsh";
						break;
					case "sh":
						result = "text/x-script.sh";
						break;
					case "shtml":
						result = "text/html";
						break;
					case "sid":
						result = "audio/x-psid";
						break;
					case "sit":
						result = "application/x-sit";
						break;
					case "skd":
						result = "application/x-koan";
						break;
					case "skm":
						result = "application/x-koan";
						break;
					case "skp":
						result = "application/x-koan";
						break;
					case "skt":
						result = "application/x-koan";
						break;
					case "sl":
						result = "application/x-seelogo";
						break;
					case "smi":
						result = "application/smil";
						break;
					case "smil":
						result = "application/smil";
						break;
					case "snd":
						result = "audio/basic";
						break;
					case "sol":
						result = "application/solids";
						break;
					case "spc":
						result = "application/x-pkcs7-certificates";
						break;
					case "spl":
						result = "application/futuresplash";
						break;
					case "spr":
						result = "application/x-sprite";
						break;
					case "sprite":
						result = "application/x-sprite";
						break;
					case "src":
						result = "application/x-wais-source";
						break;
					case "ssi":
						result = "text/x-server-parsed-html";
						break;
					case "ssm":
						result = "application/streamingmedia";
						break;
					case "sst":
						result = "application/vnd.ms-pki.certstore";
						break;
					case "step":
						result = "application/step";
						break;
					case "s":
						result = "text/x-asm";
						break;
					case "stl":
						result = "application/sla";
						break;
					case "stm":
						result = "text/html";
						break;
					case "stp":
						result = "application/step";
						break;
					case "sv4cpio":
						result = "application/x-sv4cpio";
						break;
					case "sv4crc":
						result = "application/x-sv4crc";
						break;
					case "svf":
						result = "image/x-dwg";
						break;
					case "svr":
						result = "application/x-world";
						break;
					case "swf":
						result = "application/x-shockwave-flash";
						break;
					case "talk":
						result = "text/x-speech";
						break;
					case "t":
						result = "application/x-troff";
						break;
					case "tar":
						result = "application/x-tar";
						break;
					case "tbk":
						result = "application/toolbook";
						break;
					case "tcl":
						result = "text/x-script.tcl";
						break;
					case "tcsh":
						result = "text/x-script.tcsh";
						break;
					case "tex":
						result = "application/x-tex";
						break;
					case "texi":
						result = "application/x-texinfo";
						break;
					case "texinfo":
						result = "application/x-texinfo";
						break;
					case "text":
						result = "text/plain";
						break;
					case "tgz":
						result = "application/x-compressed";
						break;
					case "tiff":
						result = "image/tiff";
						break;
					case "tif":
						result = "image/tiff";
						break;
					case "tr":
						result = "application/x-troff";
						break;
					case "trm":
						result = "application/x-msterminal";
						break;
					case "tsi":
						result = "audio/tsp-audio";
						break;
					case "tsp":
						result = "audio/tsplayer";
						break;
					case "tsv":
						result = "text/tab-separated-values";
						break;
					case "ttf":
						result = "application/x-font-ttf";
						break;
					case "turbot":
						result = "image/florian";
						break;
					case "txt":
						result = "text/plain";
						break;
					case "uil":
						result = "text/x-uil";
						break;
					case "uls":
						result = "text/iuls";
						break;
					case "unis":
						result = "text/uri-list";
						break;
					case "uni":
						result = "text/uri-list";
						break;
					case "unv":
						result = "application/i-deas";
						break;
					case "uris":
						result = "text/uri-list";
						break;
					case "uri":
						result = "text/uri-list";
						break;
					case "ustar":
						result = "multipart/x-ustar";
						break;
					case "uue":
						result = "text/x-uuencode";
						break;
					case "uu":
						result = "text/x-uuencode";
						break;
					case "vcd":
						result = "application/x-cdlink";
						break;
					case "vcf":
						result = "text/x-vcard";
						break;
					case "vcs":
						result = "text/x-vCalendar";
						break;
					case "vda":
						result = "application/vda";
						break;
					case "vdo":
						result = "video/vdo";
						break;
					case "vew":
						result = "application/groupwise";
						break;
					case "vivo":
						result = "video/vivo";
						break;
					case "viv":
						result = "video/vivo";
						break;
					case "vmd":
						result = "application/vocaltec-media-desc";
						break;
					case "vmf":
						result = "application/vocaltec-media-file";
						break;
					case "voc":
						result = "audio/voc";
						break;
					case "vos":
						result = "video/vosaic";
						break;
					case "vox":
						result = "audio/voxware";
						break;
					case "vqe":
						result = "audio/x-twinvq-plugin";
						break;
					case "vqf":
						result = "audio/x-twinvq";
						break;
					case "vql":
						result = "audio/x-twinvq-plugin";
						break;
					case "vrml":
						result = "application/x-vrml";
						break;
					case "vrt":
						result = "x-world/x-vrt";
						break;
					case "vsd":
						result = "application/x-visio";
						break;
					case "vst":
						result = "application/x-visio";
						break;
					case "vsw":
						result = "application/x-visio";
						break;
					case "w60":
						result = "application/wordperfect6.0";
						break;
					case "w61":
						result = "application/wordperfect6.1";
						break;
					case "w6w":
						result = "application/msword";
						break;
					case "wav":
						result = "audio/wav";
						break;
					case "wb1":
						result = "application/x-qpro";
						break;
					case "wbmp":
						result = "image/vnd.wap.wbmp";
						break;
					case "wcm":
						result = "application/vnd.ms-works";
						break;
					case "wdb":
						result = "application/vnd.ms-works";
						break;
					case "web":
						result = "application/vnd.xara";
						break;
					case "wiz":
						result = "application/msword";
						break;
					case "wk1":
						result = "application/x-123";
						break;
					case "wks":
						result = "application/vnd.ms-works";
						break;
					case "wmf":
						result = "windows/metafile";
						break;
					case "wmlc":
						result = "application/vnd.wap.wmlc";
						break;
					case "wmlsc":
						result = "application/vnd.wap.wmlscriptc";
						break;
					case "wmls":
						result = "text/vnd.wap.wmlscript";
						break;
					case "wml":
						result = "text/vnd.wap.wml";
						break;
					case "woff":
						result = "application/x-woff";
						break;
					case "word":
						result = "application/msword";
						break;
					case "wp5":
						result = "application/wordperfect";
						break;
					case "wp6":
						result = "application/wordperfect";
						break;
					case "wp":
						result = "application/wordperfect";
						break;
					case "wpd":
						result = "application/wordperfect";
						break;
					case "wps":
						result = "application/vnd.ms-works";
						break;
					case "wq1":
						result = "application/x-lotus";
						break;
					case "wri":
						result = "application/mswrite";
						break;
					case "wrl":
						result = "application/x-world";
						break;
					case "wrz":
						result = "model/vrml";
						break;
					case "wsc":
						result = "text/scriplet";
						break;
					case "wsdl":
						result = "text/xml";
						break;
					case "wsrc":
						result = "application/x-wais-source";
						break;
					case "wtk":
						result = "application/x-wintalk";
						break;
					case "xaf":
						result = "x-world/x-vrml";
						break;
					case "xaml":
						result = "application/xaml+xml";
						break;
					case "xap":
						result = "application/x-silverlight-app";
						break;
					case "xbap":
						result = "application/x-ms-xbap";
						break;
					case "xbm":
						result = "image/x-xbitmap";
						break;
					case "xdr":
						result = "video/x-amt-demorun";
						break;
					case "xgz":
						result = "xgl/drawing";
						break;
					case "xif":
						result = "image/vnd.xiff";
						break;
					case "xla":
						result = "application/excel";
						break;
					case "xl":
						result = "application/excel";
						break;
					case "xlb":
						result = "application/excel";
						break;
					case "xlc":
						result = "application/excel";
						break;
					case "xld":
						result = "application/excel";
						break;
					case "xlk":
						result = "application/excel";
						break;
					case "xll":
						result = "application/excel";
						break;
					case "xlm":
						result = "application/excel";
						break;
					case "xls":
						result = "application/excel";
						break;
					case "xlt":
						result = "application/excel";
						break;
					case "xlv":
						result = "application/excel";
						break;
					case "xlw":
						result = "application/excel";
						break;
					case "xm":
						result = "audio/xm";
						break;
					case "xml":
						result = "text/xml";
						break;
					case "xmz":
						result = "xgl/movie";
						break;
					case "xof":
						result = "x-world/x-vrml";
						break;
					case "xpi":
						result = "application/x-xpinstall";
						break;
					case "xpix":
						result = "application/x-vnd.ls-xpix";
						break;
					case "xpm":
						result = "image/xpm";
						break;
					case "x-png":
						result = "image/png";
						break;
					case "xsd":
						result = "text/xml";
						break;
					case "xsl":
						result = "text/xml";
						break;
					case "xsr":
						result = "video/x-amt-showrun";
						break;
					case "xwd":
						result = "image/x-xwd";
						break;
					case "xyz":
						result = "chemical/x-pdb";
						break;
					case "z":
						result = "application/x-compressed";
						break;
					case "zip":
						result = "application/zip";
						break;
					case "zsh":
						result = "text/x-script.zsh";
						break;
					default:
						result = null;
						break;
					#endregion
				}
				return result;
			}
		}
		public Path()
		{
		}
		public Path(params string[] path) :
			this()
		{
			PathLink last = null;
			if (path.NotEmpty())
			{
				path.Reverse();
				foreach (string name in path)
					last = new PathLink(name, last);
			}
			this.Last = last;
		}
		Path(PathLink last) :
			this()
		{
			this.Last = last;
		}
		public Path Copy()
		{
			Func<PathLink, PathLink> copy = null;
			copy = link => link.IsNull() ? null : new PathLink(link.Head, copy(link.Tail));
			return new Path(copy(this.Last));
		}
		public Path ResolveVariable(string name, Func<string, string> format)
		{
			return new Path(this.Last.ResolveVariable(name, format));
		}
		public Path Resolve(Path absolute)
		{
			Path result;
			switch (this[0])
			{
				case ".":
				case "..":
					result = absolute.FolderPath + this;
					break;
				default:
					result = this;
					break;
			}
			return result;
		}
		public Path Relative(Path root)
		{
			// TODO: Make something nicer then using strings
			string t = this;
			string r = root;
			return t.StartsWith(r) ? "./" + t.Substring(r.Length) : t;
		}
		public Path Skip(int count)
		{
			Func<PathLink, PathLink> helper = null;
			helper = l =>
			{
				PathLink r = l.Tail.NotNull() ? helper(l.Tail) : null;
				if (count > 0)
				{
					count--;
					r = null;
				}
				else
					r = new PathLink(l.Head, r);
				return r;
			};
			return new Path() { Last = helper(this.Last) };
		}
		#region IEquatable<Path> Members
		public bool Equals(Path other)
		{
			bool result = true;
			PathLink myTail = this.Last;
			PathLink otherTail = other.Last;
			while (myTail.NotNull() && otherTail.NotNull())
			{
				result &= myTail.Head == otherTail.Head;
				myTail = myTail.Tail;
				otherTail = otherTail.Tail;
			}
			result &= myTail.IsNull() && otherTail.IsNull();
			return result;
		}
		#endregion
		#region Object Overrides
		public override bool Equals(object other)
		{
			return other is Path && this.Equals(other as Path);
		}
		public override int GetHashCode()
		{
			int result = 0;
			PathLink tail = this.Last;
			while (tail.NotNull())
			{
				result = 33 * result ^ tail.Head.Hash();
				tail = tail.Tail;
			}
			return result;
		}
		public override string ToString()
		{
			return this.String;
		}
		#endregion
		public static Path FromPlatformPath(string path)
		{
			return new Path() { PlatformPath = path };
		}
		public static Path FromRelativePlatformPath(string path)
		{
			return Path.FromPlatformPath(System.IO.Path.GetFullPath(path));
		}
		#region static operators
		#region Casts with System.IO.FileSystemInfo
		static PathLink Create(System.IO.DirectoryInfo directory)
		{
			return directory.IsNull() ? null : new PathLink() {
				Head = directory.Name.TrimEnd('\\'),
				Tail = directory.Parent.NotNull() ? Path.Create(directory.Parent) : null
			};
		}
		static PathLink Create(System.IO.FileInfo file)
		{
			return file.IsNull() ? null : new PathLink() {
				Head = file.Name.TrimEnd('\\'),
				Tail = file.Directory.NotNull() ? Path.Create(file.Directory) : null
			};
		}
		#region Casts with System.IO.FileSystemInfo
		public static implicit operator Path(System.IO.FileSystemInfo item)
		{
			return new Path(item is System.IO.DirectoryInfo ? Path.Create(item as System.IO.DirectoryInfo) : item is System.IO.FileInfo ? Path.Create(item as System.IO.FileInfo) : null);
		}
		public static explicit operator System.IO.DirectoryInfo(Path path)
		{
			return path.NotNull() ? new System.IO.DirectoryInfo(path.PlatformPath) : null;
		}
		#endregion
		#endregion
		#region Casts with string
		public static implicit operator string(Path path)
		{
			return path.IsNull() ? null : path.String;
		}
		public static implicit operator Path(string path)
		{
			return path.IsEmpty() ? null : new Path() { String = path };
		}
		#endregion
		#region Equality Operators
		public static bool operator ==(Path left, Path right)
		{
			return left.SameOrEquals(right);
		}
		public static bool operator !=(Path left, Path right)
		{
			return !(left == right);
		}
		#endregion
		#region Add Operator
		public static Path operator +(Path left, Path right)
		{
			Path result;
			if (right.NotNull() && right.Last.NotNull())
			{
				result = right.Copy();
				if (left.NotNull())
				{
					PathLink first = result.Last;
					while (first.Tail.NotNull())
						first = first.Tail;
					first.Tail = left.Copy().Last;
				}
			}
			else if (left.NotNull())
				result = left.Copy();
			else
				result = new Path();
			result.Last = result.Last.Rebuild();
			return result;
		}
		#endregion
		#endregion
	}
}