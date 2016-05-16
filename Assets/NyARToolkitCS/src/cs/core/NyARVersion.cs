/* 
 * PROJECT: NyARToolkitCS
 * --------------------------------------------------------------------------------
 *
 * The NyARToolkitCS is C# edition NyARToolKit class library.
 * Copyright (C)2008-2012 Ryo Iizuka
 *
 * This work is based on the ARToolKit developed by
 *   Hirokazu Kato
 *   Mark Billinghurst
 *   HITLab, University of Washington, Seattle
 * http://www.hitl.washington.edu/artoolkit/
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as publishe
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * For further information please contact.
 *	http://nyatla.jp/nyatoolkit/
 *	<airmail(at)ebony.plala.or.jp> or <nyatla(at)nyatla.jp>
 * 
 */
namespace jp.nyatla.nyartoolkit.cs.core
{

    /**
     * このクラスは、NyARToolkitライブラリのバージョン情報を保持します。
     */
    public class NyARVersion
    {
        /**モジュール名*/
        public const string MODULE_NAME = "NyARToolkit";
        /**メジャーバージョン*/
        public const int VERSION_MAJOR = 5;
        /**マイナバージョン*/
        public const int VERSION_MINOR = 0;
        /**タグ*/
        public const int VERSION_TAG = 6;
        /**バージョン文字列*/
        public readonly static string VERSION_STRING = MODULE_NAME + "/" + VERSION_MAJOR + "." + VERSION_MINOR + "." + VERSION_TAG;
    }
}
