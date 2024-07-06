import { useState, useEffect } from "react";
import MailInfoBlock from "./MailInfoBlock";

function MailPanel({ isOpen, select, folder }) {
    const [mails, setMails] = useState([]);

    // Load emails
    useEffect(() => {
        (async () => {
            const res = await fetch("http://localhost:81/api/email/emails?" + new URLSearchParams({folder}).toString());
            const response = await res.json();
            setMails(response);
        })();
    }, [folder])

    // Returns the list of emails in the mail panel, I have limited this to 10 for now before I implement pagination
    return (
        <>
            { isOpen ?
                <div className="hide-scroll flex flex-col absolute h-[96vh] w-[17vw] bg-zinc-800 translate-x-[17vw] border-r-2 border-zinc-700 transition-all duration-1000 overflow-y-scroll">
                    {mails.map((mail) => (
                        <MailInfoBlock className="h-32" sender={mail.sender} subject={mail.subject} date={(new Date(mail.date)).toUTCString()} body={mail.body} select = { select } />
                    ))}
                </div>
            :
                <div className="flex flex-col absolute h-[96vh] w-0 bg-zinc-800 transition-all duration-1000">
                </div>
            }
        </>
    );
}

export default MailPanel;