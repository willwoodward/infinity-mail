import { useState } from "react";

function AIPanel({ mailContent }) {
    const [summary, setSummary] = useState("");

    // Load emails
    function get_summary() {
        console.log("Fetching summary");
        (async () => {
            const res = await fetch(`http://localhost:81/ai/query?query="Summarise the email passed as context in 3 concise bullet points."`);
            const response = await res.text();
            setSummary(response);
        })();
    }

    return (
        <>
            <div className="flex flex-col h-[96vh] bg-zinc-900 right-0 w-[17vw] transition-all absolute z-10">
                <div className="flex items-center justify-center h-14 border-b border-gray-600">
                    <div className="flex items-center justify-center">
                        <h1 className="text-l pl-4 font-bold text-white mt-4 transition-all">AI Panel</h1>
                    </div>
                </div>
                <div className="flex flex-col flex-grow">
                    <div className="px-4 pt-4">
                        <div className="text-base text-zinc-200 font-semibold">Context</div>
                        <div className="bg-zinc-800 rounded-lg mt-2">
                            <div className="text-zinc-500 px-1">Upload Context</div>
                        </div>
                    </div>
                    <div className="px-4 pt-4">
                        <div className="text-base text-zinc-200 font-semibold">History Summary</div>
                        <div className="bg-zinc-800 rounded-lg mt-2">
                            <div className={`px-1 ${summary ? "text-zinc-200" : "text-zinc-500"}`}>{summary ? summary : "Pending..."}</div>
                        </div>
                        <button onClick={() => get_summary()} className="bg-white bg-opacity-10 py-1 px-2 my-4 rounded-lg hover:bg-opacity-20 transition-all">
                            Generate Summary
                        </button>
                    </div>
                </div>
            </div>
        </>
    );
}

export default AIPanel;