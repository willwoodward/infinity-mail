import { FaEnvelope } from 'react-icons/fa';
import { FaCalendar } from 'react-icons/fa';
import { IoSettingsSharp } from 'react-icons/io5';
import { FaSearch } from 'react-icons/fa';

function Navbar() {
    return (
        <nav className="">
            <div className="flex justify-between container-fluid bg-zinc-900 h-[4vh]">
                <a className="p-2" href="/">
                    <FaEnvelope />
                </a>
                <a className="p-2" href="#">
                    <FaSearch />
                </a>
                <a className="p-2" href="#">
                    <FaCalendar />
                </a>
                <a className="p-2" href="#">
                    <IoSettingsSharp />
                </a>
            </div>
        </nav>
    );
}

export default Navbar;