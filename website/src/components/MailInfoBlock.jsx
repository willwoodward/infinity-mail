function MailInfoBlock({ sender, subject, date, emailId, select }) {
    // This renders each email preview on the sidebar
    return (
        <div className="border-b-2 border-zinc-700 hover:bg-zinc-700 hover:cursor-pointer py-2 px-2 overflow-y-visible h-[200px]" onClick={() => select(sender, subject, date, emailId)}>
            <div className="text-white font-semibold">{sender}</div>
            <div className="text-white">{subject}</div>
            <div className="text-white">{date}</div>
        </div>
    );
}

export default MailInfoBlock;