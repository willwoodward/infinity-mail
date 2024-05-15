import MailPanel from "./MailPanel";

function Sidebar({ toggle, select, isOpen }) {
    // This is for the leftmost sidebar, containing the inboxes and also the button to toggle the sidebar opening
    return (
        <>
            <div className={"transition-all duration-1000 flex flex-col h-[96vh] bg-zinc-900 " + (isOpen ? " w-[17vw] absolute z-10" : " w-0 absolute z-10")}>
                <div className="flex items-center justify-center h-14 border-b border-gray-600">
                    <div className="flex items-center justify-center">
                        <img className="h-8 w-8" src="https://tailwindui.com/img/logos/workflow-mark-indigo-500.svg" alt="Workflow" />
                    </div>
                </div>
                <div className="flex flex-col flex-grow">
                    <div className="flex">
                        <h1 className="text-l pl-4 font-bold text-white mt-4 transition-all">{isOpen ? 'Inbox' : ''}</h1>
                    </div>
                </div>
                <div className={isOpen ? "absolute inset-y-0 translate-x-[15vw] my-[43vh] transition-all duration-1000" : "absolute inset-y-0 translate-x-[-30px] my-[43vh] transition-all duration-1000"}>
                    <button className="flex items-center justify-center h-14 w-14 rounded-full transition-all bg-zinc-600 hover:bg-zinc-500 hover:outline-none hover:ring-offset-8" onClick={() => toggle()}>
                        <svg className="h-6 w-6 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke={isOpen ? "currentColor" : "none"}>
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d={isOpen ? "M6 18L18 6M6 6l12 12" : "M4 6h16M4 12h16M4 18h16"} />
                        </svg>
                    </button>
                </div>
            </div>
            <MailPanel isOpen={isOpen} select = { select } />
        </>
    );
}

export default Sidebar;