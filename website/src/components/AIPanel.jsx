function AIPanel() {
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
                            <div className="text-zinc-500 px-1">Previous conversations...</div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}

export default AIPanel;