function MainPage({ isOpen, mailContent }) {
    return (
        <div className={"bg-zinc-800 absolute transition-all duration-1000 p-3 " + (isOpen ? "translate-x-[34vw] w-[49vw] h-[96vh]" : "w-[83vw] h-[96vh] mr-64")}>
            <div className="flex justify-between">
                <div className="font-bold">{mailContent.sender}</div>
                <div className="font-bold">{mailContent.date}</div>
            </div>
            <div className="font-semibold">{mailContent.subject}</div>
            <div className="text-zinc-400">
                {
                    mailContent.body && 
                    <iframe className="w-full h-[600px]"
                        allowtransparency="true"
                        srcDoc={mailContent.body}
                        sandbox="allow-popups allow-popups-to-escape-sandbox allow-same-origin allow-scripts allow-forms"
                        csp="script-src 'none'"
                    />
                }
            </div>
        </div>
    );
}

export default MainPage;